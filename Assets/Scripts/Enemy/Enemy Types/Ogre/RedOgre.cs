using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the red ogre's behavior.
/// </summary>
public class RedOgre : Ogre
{
    protected override void AI()
    {
        base.AI();

        if (aggroed)
        {
            if (inMeleeRange)
            {
                MeleeAttack();
            }
            else
            {
                MoveInTargetDirection(currentTarget);
            }
        }
        else
        {
            Wander();
        }
    }
}
