using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines events that determine the player's immunity state.
/// </summary>
public class PlayerImmunity : MonoBehaviour
{
    private const int playerLayer = 7;
    private const int bigEnemyLayer = 8;
    private const int smallEnemyLayer = 9;

    [SerializeField] private bool overrideNormal;
    [SerializeField] private bool _isImmune = false;
    
    public bool isImmune (){ 
        return _isImmune;
    }

    /// <summary>
    /// Collision between the player and small enemies are ignored.
    /// </summary>
    public void EnablePlayerImmunity()
    {
        if (!overrideNormal)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, smallEnemyLayer, true);
            _isImmune = true;
        }
    }

    /// <summary>
    /// Collision between the player and small enemies are no longer ignored.
    /// </summary>
    public void DisablePlayerImmunity()
    {
        if (!overrideNormal)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, smallEnemyLayer, false);
            _isImmune = false;
        }
    }

    /// <summary>
    /// Collision between the player and all enemies and their projectiles are ignored.
    /// Normal immunity events are ignored.
    /// </summary>
    public void ToggleIntangibility(bool isOn)
    {
        Physics2D.IgnoreLayerCollision(playerLayer, smallEnemyLayer, isOn);
        Physics2D.IgnoreLayerCollision(playerLayer, bigEnemyLayer, isOn);
        overrideNormal = isOn;
    }
}
