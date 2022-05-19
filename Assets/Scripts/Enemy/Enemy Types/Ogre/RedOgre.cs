using UnityEngine;

/// <summary>
/// Defines the red ogre's behavior.
/// </summary>
public class RedOgre : Ogre
{
    protected override void AI()
    {
        base.AI();
        CheckMeleeRange();

        // Don't switch to different state while in a melee attack
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            if (aggroed)
            {
                if (inMeleeRange)
                {
                    MeleeAttackForeswing();
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
    }
}
