using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the debuff that deals damage over time.
/// </summary>
public class TickDamage : Debuff
{
    [SerializeField] private float damage;
    [SerializeField] private float intervalLength;
    [SerializeField] private float damageTimer;

    /// <summary>
    /// Set the damage per interval, interval length, and duration of the debuff.
    /// </summary>
    public virtual void SetDefaults(float damage, float intervalLength, float duration)
    {
        this.damage = damage;
        this.intervalLength = intervalLength;
        this.duration = duration;

        timeLeft = duration;
    }

    protected override void DebuffEffect()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= intervalLength)
        {
            gameObject.GetComponent<Health>().TakeDamage(damage);
            timeLeft -= intervalLength;
            damageTimer = 0;
        }
    }
}
