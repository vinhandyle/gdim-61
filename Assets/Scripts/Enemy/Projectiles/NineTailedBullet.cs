using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the fox fire projectile.
/// </summary>
public class NineTailedBullet : Projectile
{
    private void Awake()
    {
        GetComponent<Animator>().SetInteger("Type", Random.Range(0, 3));
    }

    protected override void AI()
    {
        if(lifetime > 10) Destroy(gameObject);
    }

    protected override void OnHitPlayerEvent(GameObject player)
    {
        base.OnHitPlayerEvent(player);
        player.GetComponent<Health>().TakeDamage(damage);
        
        TickDamage debuff = player.GetComponent<TickDamage>();
        if (debuff == null)
        {
            debuff = player.AddComponent<TickDamage>();
            debuff.SetDefaults(damage / 2, 1, 3);
        }
        else
        {
            debuff.Reapply();
        }
        Destroy(gameObject);
    }
}
