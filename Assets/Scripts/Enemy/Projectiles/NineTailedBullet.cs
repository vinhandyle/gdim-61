using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineTailedBullet : Projectile
{
    protected override void AI()
    {
        if(lifetime > 10) Destroy(gameObject);
    }

    protected override void OnHitPlayerEvent(GameObject player)
    {
        base.OnHitPlayerEvent(player);

        player.GetComponent<Health>().TakeDamage(10);
        player.GetComponent<PlayerController>().StartTickDamage(5,3,1);
        //StartCoroutine(TickDamage(player, 5, 3, 1));
        Destroy(gameObject);
    }
}
