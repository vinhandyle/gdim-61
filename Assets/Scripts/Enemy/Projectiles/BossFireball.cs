using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Projectile
{

    [SerializeField] GameObject bossFire;

    private void Awake()
    {
        
    }

    protected override void AI()
    {
        if (lifetime > 10) Destroy(gameObject);
    }

    protected override void OnHitTerrainEvent(GameObject terrain)
    {
        Instantiate(bossFire, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
