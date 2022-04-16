using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class that handles Enemy Behavior and other factors
public class Enemy : MonoBehaviour
{

    [SerializeField] EnemyBase enemyBase;
    Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        enemyBase.Instantiate(rigidBody2D);
    }

    void Update()
    {
        enemyBase.AI();
    }

    /// <summary>
    /// Damage the enemy by X amount
    /// </summary>
    /// <param name="amount"></param>
    public void DamageEnemy(double amount)
    {
        enemyBase.SetCurrentHealth(enemyBase.GetCurrentHealth() - amount);

        if (enemyBase.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }

        if(enemyBase.GetCurrentHealth() > enemyBase.GetMaxHealth())
        {
            enemyBase.SetCurrentHealth(enemyBase.GetMaxHealth());
        }
    }
}
