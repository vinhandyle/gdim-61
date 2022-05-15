using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBossExplosionAttack : MonoBehaviour
{
    [SerializeField] private float explosionDamage;

    private CapsuleCollider2D explosionHitbox;

    private void Awake()
    {
        explosionHitbox = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Health target = collision.GetComponent<Health>();
            target.TakeDamage(explosionDamage);
        }
    }
}
