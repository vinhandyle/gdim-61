using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the damaging component of a dash attack.
/// </summary>
public class DashAttack : MeleeAttack
{
    [SerializeField] private List<int> targetLayers;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayers.Any(layer => collision.gameObject.layer == layer))
        {
            base.OnTriggerEnter2D(collision);
        }
    }
}
