using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the proprties and actions related to the health stat.
/// </summary>
public class Health : MonoBehaviour
{
    public event Action OnDamageTaken;
    public event Action OnDeath;

    [SerializeField] private float _maxHealth = 1;
    public float maxHealth { get { return _maxHealth; } }

    [SerializeField] private float _health = 1;
    public float health { get { return _health; } }

    [Header("Indication of being Hit")]
    [SerializeField] private int damageFlashDelta = 0;
    [SerializeField] private float damageFlashTime = 0;
    protected Color initialColor = new Color();

    [Header("Respawn")]
    public Vector3 respawnPos;
    public bool canDie = true;
    [SerializeField] private bool respawnOnDeath;
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    private void Awake()
    {
        if (!gameObject.CompareTag("Player")) respawnPos = transform.position;

        Material material = GetComponent<SpriteRenderer>().material;
        initialColor = material.color;
        _health = maxHealth; // Comment out if testing health thresholds
    }

    /// <summary>
    /// Reduces health by the given amount. 
    /// Actions related to taking damage (or fatal damage) are also performed.
    /// </summary>
    public void TakeDamage(float amount)
    {

        Debug.Log(gameObject.name + " took " + amount + " damage");

        _health -= amount;
        OnDamageTaken?.Invoke();

        if (health <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
        else
        {
            StartCoroutine(IndicateDamage(0.05f));
        }
    }

    /// <summary>
    /// Restores the given amount of health, adjusted to not exceed the maximum health.
    /// </summary>
    public void Heal(int amount)
    {
        // Adjust for under/overhealing
        if (_health + amount > _maxHealth || amount >= _maxHealth)
        {
            amount = (int)(_maxHealth - _health);
        }
        _health += amount;
        Debug.Log(gameObject.name + " restored " + amount + " health");

    }

    public void Respawn()
    {
        gameObject.SetActive(true);

        transform.position = respawnPos;
        Heal((int)maxHealth);

        // Reset all enemies and arenas on player respawn
        if (gameObject.CompareTag("Player"))
        {
            List<Enemy> enemies = Resources.FindObjectsOfTypeAll<Enemy>().ToList();
            List<Arena> arenas = Resources.FindObjectsOfTypeAll<Arena>().ToList();

            enemies.ForEach(enemy => enemy.GetComponent<Health>().Respawn());
            enemies.ForEach(enemy => enemy.GetComponent<Enemy>().Reset());
            arenas.ForEach(arena => arena.ResetArena());

            mainCamera.Priority = 2;
        }
    }

    /// <summary>
    /// Kill the object.
    /// </summary>
    public void Die()
    {
        if (canDie)
        {
            // Clear all debuffs before unloading/respawning
            foreach (Debuff debuff in GetComponents<Debuff>()) debuff.Clear();

            // Reset attack hitboxes if died mid-attack
            foreach (MeleeAttack attackBox in GetComponentsInChildren<MeleeAttack>())
            {
                attackBox.Finish();
            }

            if (respawnOnDeath)
            {
                Respawn();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }        
    }

    #region Visual damage indicator

    IEnumerator IndicateDamage(float time)
    {
        SetBrightness(damageFlashDelta);
        yield return new WaitForSeconds(damageFlashTime);
        ResetBrightness();
    }

    protected void SetBrightness(int delta)
    {
        Material material = GetComponent<SpriteRenderer>().material;
        Color newColor = new Color(
            initialColor.r + delta > 255 ? 255 : initialColor.r + delta,
            initialColor.g + delta > 255 ? 255 : initialColor.g + delta,
            initialColor.b + delta > 255 ? 255 : initialColor.b + delta);
        material.SetColor("_Color", newColor);
    }

    protected void ResetBrightness()
    {
        Material material = GetComponent<SpriteRenderer>().material;
        material.SetColor("_Color", initialColor);
    }

    #endregion
}
