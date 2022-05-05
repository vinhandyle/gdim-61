using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the yellow ogre's behavior.
/// </summary>
public class YellowOgre : Ogre
{
    protected override void AI()
    {
        base.AI();
        CheckMeleeRange();
        UpdateChargeTimer();

        // Don't switch to different state while charging
        if (priorityStates.All(state => !anim.GetCurrentAnimatorStateInfo(0).IsName(state)))
        {
            if (aggroed)
            {
                if (inMeleeRange)
                {
                    PrepareCharge();
                }
                else
                {
                    MoveInTargetDirection(currentTarget);
                    anim.SetBool("Walking", rb.velocity != Vector2.zero);
                }
            }
            else
            {
                Wander();
            }
        }
        // While charging, check footing
        else
        {
            if (anim.GetInteger("Charging") == 2) Charge();
        }
    }
}
