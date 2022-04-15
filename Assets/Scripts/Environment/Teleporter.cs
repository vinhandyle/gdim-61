using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleports the player to a given location on contact.
/// </summary>
public class Teleporter : MonoBehaviour
{
    public Vector3 destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = destination;
        }
    }
}
