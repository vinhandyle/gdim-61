using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    private BoxCollider2D hitbox;
    [SerializeField] private float dashDamage;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy.layer == 9)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.TakeDamage(dashDamage);
        }
    }

    public void enableDashHitBox()
    {
        hitbox.enabled = true;
    }

    public void disableDashHitBox()
    {
        hitbox.enabled = false;
    }
}
