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
    [SerializeField] private float shotSpeed;
    [SerializeField] private Transform rotator;
    [SerializeField] private Transform target;

    protected override void AI()
    {
        base.AI();

        PointAtTarget(rotator, target, trackAhead);

        if (shootCDLeft <= 0)
        {
            Shoot(0, 0, shotSpeed);
            shootCDLeft = shootCD;
        }
        shootCDLeft -= Time.deltaTime;
    }
}
