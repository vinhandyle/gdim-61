using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for elemental orbs.
/// </summary>
public abstract class ElementalOrb : Enemy
{
    /// <summary>
    /// Defines events that will occur when coming into contact with a target.
    /// </summary>
    protected abstract void OnTargetContact(Transform target);

    /// <summary>
    /// Follow a target if there is one in range.
    /// </summary>
    protected virtual void WeakAI()
    {
        CheckAggro();
        GetTargetDirection(currentTarget);
        MoveTowardsTarget(currentTarget);
    }

    // TODO: Add generic methods that can be used for the mini-bosses

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (targetTags.Exists(t => collision.collider.CompareTag(t)))
        {
            OnTargetContact(collision.transform);
        }

        base.OnCollisionEnter2D(collision);
    }
}
