using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the base class for all projectiles.
/// </summary>
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected Transform origin;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected int damage;

    [Header("Timers")]
    [Tooltip("Total time this projectile has spent alive. Do not jump or reset.")]
    [SerializeField] protected float lifetime;
    [Tooltip("List of all timers used in AI. Can jump or reset.")]
    [SerializeField] protected List<float> aiTimers;

    protected virtual void Update()
    {
        lifetime += Time.deltaTime;
        aiTimers.ForEach(timer => timer += Time.deltaTime);

        AI();
    }

    /// <summary>
    /// Defines the projectile's behavior.
    /// </summary>
    protected abstract void AI();

    /// <summary>
    /// Sets the origin of the projectile instance.
    /// </summary>
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }

    /// <summary>
    /// Points the projectile towards the target.
    /// </summary>
    protected void PointToTarget(Transform target)
    {
        Vector2 direction = Vector2.zero;
        direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;
    }

    /// <summary>
    /// Defines the projectile's behavior after it hits the player.
    /// </summary>
    protected virtual void OnHitPlayerEvent(GameObject player)
    {
        Debug.Log(gameObject.name + " hit " + player.name);
    }

    /// <summary>
    /// Defines the projectile's behavior after it hits terrain.
    /// </summary>
    protected virtual void OnHitTerrainEvent(GameObject terrain)
    {
        Debug.Log(gameObject.name + " hit " + terrain.name);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnHitPlayerEvent(collision.gameObject);
        }
        if(collision.CompareTag("Ground"))
        {
            OnHitTerrainEvent(collision.gameObject);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    { 
        // ?
    }
}
