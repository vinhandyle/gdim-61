using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the ogre enemy. 
/// Not abstract since we need it on the root prefab for generic animation events.
/// </summary>
public class Ogre : Enemy
{
    [Header("Ogre Melee")]
    [Space] [Space] [Space]
    [SerializeField] protected MeleeAttack meleeAttack;
    [SerializeField] protected float chargeSpeedMult;
    [Tooltip("Set negative if you want infinite charge time.")]
    [SerializeField] protected float maxChargeTime;
    [SerializeField] protected float chargeTimer;

    [Header("Ogre Ranged")]
    [SerializeField] protected Transform rotator;
    [SerializeField] protected bool loopShoot; 
    [SerializeField] protected int ammoMax;
    [SerializeField] protected int ammoLeft;
    [SerializeField] protected int ammoRetrieved;

    protected List<string> priorityStates = new List<string>(){ "Charge Windup", "Charge" };

    protected override void Awake()
    {
        base.Awake();
        ammoLeft = ammoMax;
        chargeTimer = maxChargeTime;
    }

    protected override void AI()
    {
        CheckAggro();
    }

    #region Melee Attack

    /// <summary>
    /// Starts the ogre's melee attack.
    /// </summary>
    protected void MeleeAttackForeswing()
    {
        GetTargetDirection(currentTarget);
        anim.SetBool("Melee", true);
        anim.SetBool("Walking", false);
        meleeAttack.Foreswing();
    }

    /// <summary>
    /// Animation event for melee attack hit.
    /// </summary>
    protected void MeleeAttackHit()
    {
        meleeAttack.Hit();
    }

    /// <summary>
    /// Animation event for melee attack backswing.
    /// </summary>
    protected void MeleeAttackBackswing()
    {
        meleeAttack.Backswing();
    }

    /// <summary>
    /// Animation event for melee attack finish.
    /// </summary>
    protected void MeleeAttackFinish()
    {
        meleeAttack.Finish();
        anim.SetBool("Melee", false);
    }

    #endregion

    #region Ranged Attack

    /// <summary>
    /// Aims at the current target, or in the direction of the previous target
    /// </summary>
    protected virtual void AimAtTarget()
    {
        if (currentTarget != null)
        {
            PointAtTarget(rotator, currentTarget);
            GetTargetDirection(currentTarget);
        }
    }

    /// <summary>
    /// Starts the ogre's ranged attack.
    /// </summary>
    protected void RangedAttackStart()
    {
        anim.SetInteger("Throwing", 1);
    }

    /// <summary>
    /// The point during the attack when the projectile is actually launched.
    /// </summary>
    protected void ThrowWeapon()
    {
        Shoot(0, 0, shotSpeed);
        ammoLeft--;
    }

    /// <summary>
    /// Animation event for ranged attack backswing.
    /// </summary>
    protected void RangedAttackFinish()
    {
        // Since the second shot does not check ammo, we dont check > 0
        anim.SetInteger("Throwing", (ammoLeft > 1) ? 2 : 0);
    }

    /// <summary>
    /// Called by the projectile when it returns to the ogre.
    /// </summary>
    public void CatchWeapon()
    {
        ammoRetrieved++;
    }

    #endregion

    #region Charge

    /// <summary>
    /// Sets the direction of the charge and telegraphs for the player.
    /// </summary>
    protected void PrepareCharge()
    {
        anim.SetBool("Walking", false);
        anim.SetInteger("Charging", 1);
        rb.velocity = Vector2.zero;
        GetTargetDirection(currentTarget);
    }

    /// <summary>
    /// Charge in the current direction. Once in motion, cannot change direction.
    /// </summary>
    protected void Charge()
    {
        MoveInTargetDirection(null, chargeSpeedMult);

        switch (anim.GetInteger("Charging"))
        {
            // Was winding up
            case 1:
                chargeTimer = maxChargeTime;
                anim.SetInteger("Charging", 2);
                break;

            // Was charging
            case 2:
                anim.SetInteger("Charging", (rb.velocity == Vector2.zero) ? 0 : 2);
                break;
        }
    }

    /// <summary>
    /// Stop the charge after a certain amount of time if the ogre hasn't stopped at an edge yet.
    /// </summary>
    protected void UpdateChargeTimer()
    {
        if (maxChargeTime < 0) return;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Charge"))
        {
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                anim.SetInteger("Charging", 0);
            }
        }
    }

    #endregion
}
