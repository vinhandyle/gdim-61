using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the nine-tailed fox's behavior.
/// </summary>
public class NineTailedFox : Enemy
{
    [SerializeField] private float shootCD;
    [SerializeField] private float shootCDLeft;
    [SerializeField] private Transform rotator;

    protected override void AI()
    {
        CheckAggro();
        CheckShootRange();
        if (currentTarget != null) PointAtTarget(rotator, currentTarget);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            if (aggroed)
            {
                if (inShootRange)
                {
                    if (!anim.GetBool("Attacking") && shootCDLeft <= 0) FoxFireStart();
                    anim.SetBool("Walking", false);
                    shootCDLeft -= Time.deltaTime;
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

    /// <summary>
    /// Starts the nine-tailed fox's attack.
    /// </summary>
    private void FoxFireStart()
    {
        GetTargetDirection(currentTarget);
        anim.SetBool("Attacking", true);
    }

    /// <summary>
    /// The point during the attack when the projectile is actually launched.
    /// </summary>
    private void FoxFireRelease()
    {
        Shoot(0, 0, shotSpeed);
    }

    /// <summary>
    /// Ends the nine-tailed fox's attack.
    /// </summary>
    private void FoxFireFinish()
    {
        shootCDLeft = shootCD;
        anim.SetBool("Attacking", false);
    }
}
