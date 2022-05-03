using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the yellow ogre's behavior.
/// </summary>
public class YellowOgre : Ogre
{
    protected override void AI()
    {
        base.AI();

        if (aggroed)
        {
            Charge();
        }
        else
        {
            Wander();
        }
    }
}
