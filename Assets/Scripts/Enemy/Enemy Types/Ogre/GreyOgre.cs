using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the grey ogre's behavior.
/// </summary>
public class GreyOgre : Ogre
{
    [SerializeField] private int chosenMove;

    protected override void Awake()
    {
        base.Awake();
        if (!priorityStates.Contains("Throw")) priorityStates.Add("Throw");
        if (!priorityStates.Contains("Throw Loop")) priorityStates.Add("Throw Loop");
    }

    protected override void AI()
    {
        base.AI();
        CheckMeleeRange();
        CheckShootRange();
        AimAtTarget();
        UpdateChargeTimer();

        // Don't switch to different state while charging or throwing
        if (priorityStates.All(state => !anim.GetCurrentAnimatorStateInfo(0).IsName(state)))
        {
            if (aggroed)
            {
                if (chosenMove == 0) chosenMove = Random.Range(1, 3);

                switch (chosenMove)
                {
                    case 0:
                        chosenMove = Random.Range(1, 2);
                        break;

                    case 1:
                        if (inMeleeRange)
                        {
                            PrepareCharge();
                            chosenMove = 0;
                        }
                        else
                        {
                            MoveInTargetDirection(currentTarget);
                            anim.SetBool("Walking", rb.velocity != Vector2.zero);
                        }
                        break;

                    case 2:
                        if (ammoLeft > 0)
                        {
                            if (inShootRange)
                            {
                                if (anim.GetInteger("Throwing") == 0 && ammoLeft > 0) RangedAttackStart();
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
                            chosenMove = 0;
                            ammoLeft = ammoMax;
                            ammoRetrieved = 0;
                        }                        
                        break;
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
