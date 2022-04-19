using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the player's basic melee attack.
/// </summary>
public class BasicAttack : MonoBehaviour
{
    private BoxCollider2D hitbox;
    private SpriteRenderer spriteRenderer;

    [Header("Attack Stats")]
    [SerializeField] private float damage;

    [Header("Sync with Animation")]
    [SerializeField] private float foreswingDuration;
    [SerializeField] private float hitDuration;
    [SerializeField] private float backswingDuration;

    [Header("Placeholder for Animation")]
    [SerializeField] private List<Sprite> sprites;

    public bool inProcess;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health enemy = collision.GetComponent<Health>();
            enemy.TakeDamage(damage);
        }
    }

    /// <summary>
    /// Call to begin the attack process.
    /// </summary>
    public void Initiate()
    {
        inProcess = true;
        StartCoroutine(ForeSwing());
    }

    /// <summary>
    /// The segment before the attack lands.
    /// </summary>
    IEnumerator ForeSwing()
    {
        spriteRenderer.sprite = sprites[0];

        yield return new WaitForSeconds(foreswingDuration);

        StartCoroutine(Hit());
    }

    /// <summary>
    /// The segment during which the attack deals damage.
    /// </summary>
    IEnumerator Hit()
    {
        hitbox.enabled = true;
        spriteRenderer.sprite = sprites[1];

        yield return new WaitForSeconds(hitDuration);

        StartCoroutine(BackSwing());
    }

    /// <summary>
    /// The segment after the attack but before control is returned to the player.
    /// </summary>
    IEnumerator BackSwing()
    {
        hitbox.enabled = false;
        spriteRenderer.sprite = sprites[2];

        yield return new WaitForSeconds(backswingDuration);

        spriteRenderer.sprite = null;
        inProcess = false;
    }
}
