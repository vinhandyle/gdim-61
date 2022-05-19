using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines places where the player will respawn at.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().respawnPos = new Vector3(transform.position.x, transform.position.y, collision.transform.position.z);
            FindObjectOfType<GameStateManager>().GetComponent<DebugMenuUI>().checkpointNum = FindObjectOfType<CheckpointArray>().checkpoints.IndexOf(this);
        }
    }
}
