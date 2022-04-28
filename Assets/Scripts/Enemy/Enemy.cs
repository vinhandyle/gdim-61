using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that handles Enemy Behavior and other factors.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] protected EnemyStats stats;
    protected Rigidbody2D rb;
    protected Vector2 direction;

    [Header("Projectile Manager")]
    [SerializeField] protected List<Transform> shootPoints;
    [SerializeField] protected List<GameObject> projectiles;

    [Header("Knockback")]
    [SerializeField] protected float kbHorizontal = 75;
    [SerializeField] protected float kbVertical = 25;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
    }

    protected virtual void Update()
    {
        if (rb.velocity.x > 0) direction.x = 1;
        else if (rb.velocity.x < 0) direction.x = -1;

        AI();
    }

    /// <summary>
    /// Defines the enemy's behavior.
    /// </summary>
    protected abstract void AI();

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
