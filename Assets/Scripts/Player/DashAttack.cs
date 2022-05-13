using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCollision : MonoBehaviour
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
        
        GameObject childEnemy = collision.gameObject;
        GameObject parentEnemy = childEnemy.transform.parent.gameObject;
        if (parentEnemy.layer == 9)
        {
            Health enemyHealth = parentEnemy.GetComponent<Health>();
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
