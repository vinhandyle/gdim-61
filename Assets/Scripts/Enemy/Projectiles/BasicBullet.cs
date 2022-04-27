using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A basic projectile that deals damage to the player on hit. Destroyed on contact or after 10 seconds.
/// </summary>
public class BasicBullet : Projectile
{
    protected override void AI()
    {
        if(lifetime > 10) Destroy(gameObject);
    }

    protected override void OnHitPlayerEvent(GameObject player)
    {
        base.OnHitPlayerEvent(player);

        player.GetComponent<Health>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
