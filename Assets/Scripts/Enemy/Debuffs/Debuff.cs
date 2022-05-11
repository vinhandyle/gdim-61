using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for debuffs.
/// All debuffs should implement a public SetDefaults method.
/// </summary>
public abstract class Debuff : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] protected float timeLeft;

    protected void Update()
    {
        DebuffEffect();

        if (duration >= 0 && timeLeft <= 0) Clear();
    }

    /// <summary>
    /// Defines the debuff's effect.
    /// </summary>
    protected abstract void DebuffEffect();

    /// <summary>
    /// Remove the debuff and its effects.
    /// </summary>
    public virtual void Clear()
    {
        Debug.Log("Cleared " + this + " on " + gameObject.name);
        Destroy(this);
    }

    /// <summary>
    /// Reset the debuff's timer.
    /// </summary>
    public virtual void Reapply()
    {
        timeLeft = duration;
        Debug.Log("Reapplied " + this + " on " + gameObject.name);
    }
}
