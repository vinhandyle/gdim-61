using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the ogre enemy.
/// </summary>
public abstract class Ogre : Enemy
{
    [Header("Ogre-specific")]
    [SerializeField] private int level;
    [SerializeField] protected int projAmt;
    [SerializeField] protected bool inMeleeRange;

    protected override void AI()
    {
        CheckAggro();
    }

    /// <summary>
    /// Moves back and forth along the ground.
    /// </summary>
    protected void Wander()
    {
        MoveInTargetDirection(null);
    }

    /// <summary>
    /// Area attack in front of the ogre.
    /// </summary>
    protected void MeleeAttack()
    { 
    
    }

    /// <summary>
    /// Throws a club towards the player. Must wait until all clubs return before next move.
    /// </summary>
    protected void RangedAttack()
    { 
    
    }

    /// <summary>
    /// Charge towards the player.
    /// </summary>
    protected void Charge()
    { 
    
    }
}
