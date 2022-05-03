using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This enemy will chase the player if they get too close.
/// </summary>
public class SpiritEnemy : Enemy
{
    [SerializeField] private float trackingDistance;
    [SerializeField] private float lostInterestDistance;
    private bool isTracking = false;

    protected override void AI()
    {
        Transform closestTarget = GetNearestTarget(trackingDistance);
        if (closestTarget != null)
        {
            isTracking = true;
        }

        if(isTracking)
        {
            Transform closestTarget2 = GetNearestTarget(lostInterestDistance);
            if (closestTarget2 != null)
            {
                Vector2 direction = closestTarget2.position - transform.position;
                direction.Normalize();
                direction *= stats.speed;
                rb.velocity = direction;

                Vector2 distanceToTarget = closestTarget2.position - transform.position;
                float dSqrToTarget = distanceToTarget.sqrMagnitude;
            }
            else
            {
                isTracking = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// Returns the closest player to the enemy within the given distance.
    /// </summary>
    protected override Transform GetNearestTarget(float distance)
    {
        if (transform == null) return null;

        Transform bestTarget = null;
        float closestDistanceSqr = distance;
        Vector3 currentPosition = transform.position;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Get all GameObjects that are tagged as "Players"
        foreach (GameObject player in players) // For each player in the array of GameObjects
        {
            Vector2 directionToTarget = player.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = player.transform;
            }
        }

        return bestTarget;
    }
}