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
        StartCoroutine(TickDamage(player, 5, 3, 1));
        //Destroy(gameObject);
    }

     protected IEnumerator TickDamage(GameObject player, int damagePerTick, int numberOfTicks, float timeBetweenTicks)
    {
        yield return new WaitForSeconds(timeBetweenTicks);
        if (numberOfTicks > 0)
        {
            player.GetComponent<Health>().TakeDamage(damagePerTick);
            StartCoroutine(TickDamage(player, damagePerTick, numberOfTicks - 1, timeBetweenTicks));
        }
    } 
}
