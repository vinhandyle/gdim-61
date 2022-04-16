using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : ScriptableObject
{
    [SerializeField] protected double damage;
    [SerializeField] protected double maxHealth;
    protected double currentHealth;
    [SerializeField] protected double speed;
    protected Rigidbody2D rigidbody2D;

    // Designed to allow the children of this class to
    // get access to the RigidBody
    public virtual void Instantiate(Rigidbody2D rigidbody2D)
    {
        currentHealth = maxHealth;
        this.rigidbody2D = rigidbody2D;
    }

    /// <summary>
    /// Determines the AI of the enemy as it moves through the game world
    /// </summary>
    public abstract void AI();

    // Getters and Setters
    public double GetDamage() { return damage; }

    public double GetMaxHealth() { return maxHealth; }

    public double GetCurrentHealth() { return currentHealth; }
    public void SetCurrentHealth(double amount) { currentHealth = amount; }

    public double GetSpeed() { return speed; }
}
