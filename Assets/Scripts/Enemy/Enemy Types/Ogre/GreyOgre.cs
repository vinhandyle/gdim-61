using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the grey ogre's behavior.
/// </summary>
public class GreyOgre : Ogre
{
    protected override void AI()
    {
        base.AI();

        if (aggroed)
        {
            float rand = Random.Range(0, 1);
            if (rand > 0.5f)
            {
                Charge();
            }
            else
            {
                RangedAttack(); // xN
            }
        }
        else
        {
            Wander();
        }
    }
}
