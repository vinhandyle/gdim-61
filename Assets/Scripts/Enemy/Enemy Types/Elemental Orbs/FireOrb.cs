using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the weak fire orb.
/// </summary>
public class FireOrb : ElementalOrb
{
    protected override void AI()
    {
        WeakAI();
    }

    protected override void OnTargetContact(Transform target)
    {
        return;
    }
}
