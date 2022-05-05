using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the ogre's boomerang-like projectile.
/// </summary>
public class OgreClub : Projectile
{
    [SerializeField] private float acceleration;
    [SerializeField] private float lifetimeMax;
    [SerializeField] private bool returning;

    protected override void AI()
    {
        rb.velocity -= (Vector2)transform.right * acceleration * Time.deltaTime;
        returning = Vector2.Dot(rb.velocity, transform.right) < 0;

        if (lifetime >= lifetimeMax) ReturnToOrigin();
    }

    /// <summary>
    /// Removes the projectile and updates its owner.
    /// </summary>
    private void ReturnToOrigin()
    {
        origin.GetComponent<Ogre>().CatchWeapon();
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (returning && collision.transform == origin)
        {
            ReturnToOrigin();
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);

        if (returning && collision.transform == origin)
        {
            ReturnToOrigin();
        }
    }
}
