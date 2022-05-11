using System;
using System.Collections;
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
    public Transform respawnPoint;
    [SerializeField] private bool canRespawn;

    private void Awake()
    {
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
        StartCoroutine(IndicateDamage(0.05f));

        if (health <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    /// <summary>
    /// Restores the given amount of health, adjusted to not exceed the maximum health.
    /// </summary>
    public virtual void Heal(int amount)
    {
        if (_health + amount <= _maxHealth)
        {
            _health += amount;
            Debug.Log(gameObject.name + " restored " + amount + " health");
        }
    }

    /// <summary>
    /// Kill the object.
    /// </summary>
    private void Die()
    {
        if (canRespawn)
        {
            // Clear all debuffs before respawning
            foreach (Debuff debuff in GetComponents<Debuff>()) debuff.Clear();

            transform.position = respawnPoint.position;
            Heal((int) maxHealth);
        }
        else
        {
            Destroy(gameObject);
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
