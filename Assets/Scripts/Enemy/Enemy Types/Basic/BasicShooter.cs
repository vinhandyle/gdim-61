using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic enemy that shoots a projectile at the player every specified time interval (in seconds).
/// </summary>
public class BasicShooter : BasicEnemy
{
    [SerializeField] private bool trackAhead;
    [SerializeField] private float shootCD;
    [SerializeField] private float shootCDLeft;
    [SerializeField] private Transform rotator;

    protected override void AI()
    {
        base.AI();
        CheckAggro();
        CheckShootRange();
        PointAtTarget(rotator, currentTarget, trackAhead);

        if (shootCDLeft <= 0)
        {
            Shoot(0, 0, shotSpeed);
            shootCDLeft = shootCD;
        }
        shootCDLeft -= Time.deltaTime;
    }
}
