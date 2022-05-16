using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines objects that will instantly kill the player on contact.
/// </summary>
public class Hazard : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().Die();
        }
    }
}
