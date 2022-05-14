using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the fire orb mini-boss.
/// </summary>
public class FireBoss : ElementalOrb
{
    protected override void AI()
    {
        // TODO
    }

    protected override void OnTargetContact(Transform target)
    {
        TickDamage debuff = target.GetComponent<TickDamage>();
        if (debuff == null)
        {
            debuff = target.gameObject.AddComponent<TickDamage>();
            debuff.SetDefaults(stats.damage / 2, 1, 4);
        }
    }
}
