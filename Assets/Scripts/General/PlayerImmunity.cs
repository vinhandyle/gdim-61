using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImmunity : MonoBehaviour
{
    private const int playerLayer = 7;
    private const int smallEnemyLayer = 9;

    [SerializeField] private bool _isImmune = false;
    
    public bool isImmune (){ 
        return _isImmune;
    }

    public void EnablePlayerImmunity(){
        Physics2D.IgnoreLayerCollision(playerLayer, smallEnemyLayer, true);
        _isImmune = true;
    }

    public void DisablePlayerImmunity(){
        Physics2D.IgnoreLayerCollision(playerLayer, smallEnemyLayer, false);
        _isImmune = false;
    }
}
