using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This enemy does not do anything
// Simply a damage sponge

[CreateAssetMenu(fileName = "BasicEnemy", menuName = "Enemy Types/BasicEnemy", order = 2)]
public class BasicEnemy : EnemyBase
{
    public override void AI()
    {
        
    }
}
