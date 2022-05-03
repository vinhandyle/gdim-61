using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The class that handles Enemy Behavior and other factors.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] protected EnemyStats stats;
    protected Rigidbody2D rb;
    [SerializeField] protected Vector2 direction;

    [Header("Projectile Manager")]
    [SerializeField] protected List<Transform> shootPoints;
    [SerializeField] protected List<GameObject> projectiles;

    [Header("Aggro")]
    [SerializeField] protected List<string> targetTags;
    [SerializeField] protected Transform currentTarget;
    [SerializeField] protected float aggroRange;
    [SerializeField] protected float deaggroRange;
    [SerializeField] protected bool aggroed;

    [SerializeField] protected EnemyGroundCheck enemyGroundCheck;

    [Header("Knockback")]
    [Tooltip("Flame dash and shell smash behave differently with small and large enemies.")]
    [SerializeField] protected bool small;
    [SerializeField] protected float kbHorizontal = 75;
    [SerializeField] protected float kbVertical = 25;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //direction = Vector2.zero;
    }

    protected virtual void Update()
    {
        AI();
    }

    /// <summary>
    /// Defines the enemy's behavior.
    /// </summary>
    protected abstract void AI();

    #region Projectile System
    /// <summary>
    /// Shoot a projectile from a shoot point at the given speed.
    /// </summary>
    /// <param name="shootPointIndex">Index of the shoot point in the component list.</param>
    /// <param name="projIndex">Index of the projectile in the component list.</param>
    /// <returns>The projectile gameobject.</returns>
    protected virtual GameObject Shoot(int shootPointIndex, int projIndex, float speed)
    {
        Transform shootPoint = shootPoints[shootPointIndex];
        GameObject proj = Instantiate(projectiles[projIndex], shootPoint.position, shootPoint.rotation).gameObject;
        proj.GetComponent<Rigidbody2D>().velocity = proj.transform.right * speed;
        return proj;
    }

    /// <summary>
    /// Rotates the given focal point so that its transform.right is pointing towards the target point.
    /// </summary>
    /// <param name="trackAhead">Set true if the focal point should track where target will be, based on the target's velocity</param>
    protected virtual void PointAtTarget(Transform focus, Transform target, bool trackAhead = false)
    {
        Vector2 direction = Vector2.zero;
        if (trackAhead && target.GetComponent<Rigidbody2D>() != null)
        {
            Vector3 deltaPos = target.GetComponent<Rigidbody2D>().velocity; // Not the most effective
            direction = (target.position + deltaPos - transform.position).normalized;
        }
        else
        {
            direction = (target.position - transform.position).normalized;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        focus.eulerAngles = Vector3.forward * angle;
    }

    #endregion

    #region Aggro System

    /// <summary>
    /// Returns all potential targets within the given the distance from the enemy.
    /// </summary>
    protected virtual List<GameObject> GetAllTargets(float distance)
    {
        if (transform == null) return null;

        List<GameObject> targets = new List<GameObject>();
        foreach (string tag in targetTags)
        {
            targets.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        return targets;
    }

    /// <summary>
    /// Returns the closest target to the enemy within the given distance.
    /// </summary>
    protected virtual Transform GetNearestTarget(float distance)
    {
        if (transform == null) return null;

        Transform bestTarget = null;
        float closestDistanceSqr = distance;
        Vector3 currentPosition = transform.position;
        
        // Find nearest potential target
        foreach (GameObject target in GetAllTargets(distance))
        {
            Vector2 directionToTarget = target.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = target.transform;
            }
        }

        return bestTarget;
    }

    /// <summary>
    /// Sets the cardinal direction of the target relative to the enemy.
    /// </summary>
    protected virtual void GetTargetDirection(Transform target)
    {
        if (target == null) return;

        int x = (target.position.x > transform.position.x) ? 1 : -1;
        int y = (target.position.y > transform.position.y) ? 1 : -1;

        if (x != direction.x) FlipEnemy();

        direction.x = x;
        direction.y = y;
    }

    /// <summary>
    /// Sets the enemy's aggro status and its current target.
    /// </summary>
    protected virtual void CheckAggro()
    {
        List<GameObject> objectsInBothRanges = GetAllTargets(deaggroRange);
        Transform closest = GetNearestTarget(aggroRange);

        if (closest == null)
        {
            // Check if current target is between aggro and deaggro ranges
            if (objectsInBothRanges.All(o => o.transform != currentTarget))
            {
                currentTarget = null;
                aggroed = false;
            }
        }
        else
        {
            currentTarget = closest;
            aggroed = true;
        }
    }

    #endregion

    #region Free to Move Directly to Target

    /// <summary>
    /// The enemy moves in a straight line towards the target.
    /// </summary>
    /// <param name="speedMult">Amount to multiply the enemy's base speed by.</param>
    protected virtual void MoveTowardsTarget(Transform target, float speedMult = 1)
    {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        direction *= stats.speed * speedMult;
        rb.velocity = direction;
    }

    #endregion

    #region Restricted to Moving Along Ground

    /// <summary>
    /// Check if the enemy has "stable" footing on the ground. Changes direction if not.
    /// </summary>
    protected virtual void CheckGround()
    {
        enemyGroundCheck.onGround = Physics2D.Raycast(enemyGroundCheck.groundCheck.position, Vector2.down, enemyGroundCheck.radius, enemyGroundCheck.isGround);

        // Enemy will not change direction if it is following a target and is not afraid of falling
        //  OR if it has recently changed direction
        if (!enemyGroundCheck.onGround && !(aggroed && enemyGroundCheck.fearless) && !enemyGroundCheck.repositioning)
        {
            FlipEnemy();
            direction.x *= -1;
            enemyGroundCheck.repositioning = true;
        }
        else if (enemyGroundCheck.onGround)
        {
            enemyGroundCheck.repositioning = false;
        }
    }

    /// <summary>
    /// The enemy moves horizontally in the target's direction relative to it.
    /// </summary>
    /// <param name="speedMult">Amount to multiply the enemy's base speed by.</param>
    protected virtual void MoveInTargetDirection(Transform target, float speedMult = 1)
    {
        GetTargetDirection(target);
        CheckGround();

        // Enemy will stop at edge if following target but scared of falling
        if (aggroed && !enemyGroundCheck.onGround && !enemyGroundCheck.fearless)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(direction.x * stats.speed * speedMult, rb.velocity.y);
        }
    }

    #endregion

    #region Misc

    /// <summary>
    /// Flips the enemy to face the opposite direction
    /// </summary>
    void FlipEnemy()
    {
        Vector3 oppDirection = transform.localScale;
        oppDirection.x *= -1;
        transform.localScale = oppDirection;
    }

    #endregion

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.collider.GetComponent<PlayerController>();

        if(player != null)
        {
            player.GetComponent<Health>().TakeDamage(stats.damage);
            player.AddForce(new Vector2(direction.x * kbHorizontal, kbVertical), 0.1f);
            //collision.collider.attachedRigidbody.AddForce(new Vector2(direction.x * kbHorizontal, kbVertical));
        }
    }
}

/// <summary>
/// ?
/// </summary>
[System.Serializable]
public class EnemyGroundCheck
{
    public LayerMask isGround;
    public Transform groundCheck;
    public float radius;
    public bool fearless;
    public bool onGround;
    public bool repositioning;
}
