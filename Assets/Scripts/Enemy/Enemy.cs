using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that handles Enemy Behavior and other factors.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private EnemyStats stats;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        AI();
    }

    /// <summary>
    /// Defines the enemy's behavior.
    /// </summary>
    protected abstract void AI();
}
