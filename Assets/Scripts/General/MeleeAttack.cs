using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines all attacks that are stationary relative to their owner.
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    protected BoxCollider2D hitbox;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected List<string> targetTags;

    [Header("Attack Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected int healAmt;

    [Header("Attack Phase Visualizer")]
    [SerializeField] protected List<Sprite> sprites;

    public bool inProcess;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTags.Any(tag => collision.CompareTag(tag)))
        {
            GetComponentInParent<Health>().Heal(healAmt);
            Health target = collision.GetComponent<Health>();
            target.TakeDamage(damage);
        }
    }

    /// <summary>
    /// The segment before the attack lands.
    /// </summary>
    public void Foreswing()
    {
        inProcess = true;
        //spriteRenderer.sprite = sprites[0]; // Comment out if not testing
    }

    /// <summary>
    /// The segment during which the attack deals damage.
    /// </summary>
    public void Hit()
    {
        hitbox.enabled = true;
        spriteRenderer.sprite = sprites[1]; // Comment out if not testing
    }

    /// <summary>
    /// The segment after the attack but before control is returned to the player.
    /// </summary>
    public void Backswing()
    {
        hitbox.enabled = false;
        //spriteRenderer.sprite = sprites[2]; // Comment out if not testing
    }

    /// <summary>
    /// Return control back to the player.
    /// </summary>
    public void Finish()
    {
        hitbox.enabled = false; // Just in case there is no backswing
        spriteRenderer.sprite = null;
        inProcess = false;
    }
}
