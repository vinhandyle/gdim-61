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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if(player != null)
        {
            player.GetComponent<Health>().TakeDamage(stats.damage);
            collision.collider.attachedRigidbody.AddForce(new Vector2(direction.x * 750, 250));
        }
    }
}
