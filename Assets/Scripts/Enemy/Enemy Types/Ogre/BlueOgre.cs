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
        CheckShootRange();
        AimAtTarget();

        // Don't switch to different state while throwing
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            if (aggroed)
            {
                if (ammoLeft > 0)
                {
                    if (inShootRange)
                    {
                        if (!anim.GetBool("Throwing")) RangedAttackStart();
                        anim.SetBool("Walking", false);
                    }
                    else
                    {
                        MoveInTargetDirection(currentTarget);
                        anim.SetBool("Walking", rb.velocity != Vector2.zero);
                    }
                }
                // Wait until all projectiles are retrieved
                else if (ammoRetrieved >= ammoMax)
                {
                    ammoLeft = ammoMax;
                    ammoRetrieved = 0;
                }
            }
            else
            {
                Wander();
            }
        }
    }
}
