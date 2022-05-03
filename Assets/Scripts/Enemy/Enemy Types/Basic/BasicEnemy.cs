using UnityEngine;

/// <summary>
/// This enemy does not do anything and is simply a damage sponge.
/// </summary>
public class BasicEnemy : Enemy
{
    [SerializeField] private bool immortal;

    protected override void AI()
    {
        if (immortal)
        {
            GetComponent<Health>().Heal(1);
        }
    }
}
