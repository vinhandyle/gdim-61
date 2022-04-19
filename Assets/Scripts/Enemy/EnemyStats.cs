using UnityEngine;

/// <summary>
/// Defines default enemy stats. No setters!
/// </summary>
public abstract class EnemyStats : ScriptableObject
{
    [SerializeField] private int _damage = 1;
    public int damage { get { return _damage; } }

    [SerializeField] private float _speed = 1;
    public float speed { get { return _speed; } }
}