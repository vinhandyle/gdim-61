using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the blue ogre's behavior.
/// </summary>
public class BlueOgre : Ogre
{
    protected override void AI()
    {
        base.AI();

        if (aggroed)
        {
            RangedAttack();
        }
        else
        {
            Wander();
        }
    }
}
