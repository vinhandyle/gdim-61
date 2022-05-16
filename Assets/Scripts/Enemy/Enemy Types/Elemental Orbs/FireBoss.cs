using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the fire orb mini-boss.
/// </summary>
public class FireBoss : ElementalOrb
{
    [SerializeField] private Transform rotator;
    [SerializeField] private CapsuleCollider2D explosionHitbox;
    // Testing only
    [SerializeField] private SpriteRenderer explosionSprite;

    // Placeholder code for fuse indication
    protected Color initialColor = new Color();

    [SerializeField] private bool hasShot = false;
    [SerializeField] private bool explosionActive = false;
    [SerializeField] private bool ultimateAttackFinished = false;

    protected override void Awake()
    {
        base.Awake();
        initialColor = GetComponent<SpriteRenderer>().material.color;
    }

    protected override void AI()
    {
        CheckAggro();
        CheckShootRange();

        if (currentTarget != null) PointAtTarget(rotator, currentTarget);
        // TODO: better movement code for the boss
        MoveTowardsTarget(currentTarget, 1);

        // Basic fireball attack
        if (!hasShot)
        {
            StartCoroutine(FireballAttack());
        }

        // Explosion Attack
        Transform explosionTarget = GetNearestTarget(5);
        if (explosionTarget && !explosionActive)
        {
            StartCoroutine(ExplosionAttack());
        }

        // Ultimate attack trigger
        Health bossHealth = GetComponent<Health>();
        if (bossHealth.health < bossHealth.maxHealth / 2 && !ultimateAttackFinished)
        {
            StartCoroutine(UltimateAttack());
        }                
    }

    private IEnumerator FireballAttack()
    {
        Shoot(0, 0, shotSpeed);
        Shoot(1, 0, shotSpeed);
        Shoot(2, 0, shotSpeed);

        hasShot = true;
        yield return new WaitForSeconds(5);
        hasShot = false;
    }

    private IEnumerator ExplosionAttack()
    {
        explosionActive = true;

        // Temporary fuse "animation"
        for (int i = 0; i < 5; i++)
        {
            SetBrightness(100);
            yield return new WaitForSeconds(0.1f);
            ResetBrightness();
            yield return new WaitForSeconds(0.4f);
        }
        // After the fuse time, activate hitbox
        explosionHitbox.enabled = true;
        explosionSprite.enabled = true;

        // Deactive hitbox
        yield return new WaitForSeconds(0.5f);
        explosionHitbox.enabled = false;
        explosionSprite.enabled = false;

        // Don't let the boss use the explosion until another 2 seconds
        yield return new WaitForSeconds(2f);
        explosionActive = false;
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

    private IEnumerator UltimateAttack()
    {
        ultimateAttackFinished = true;
        for (int i = 0; i < 3; i++)
        {
            Shoot(3, 0, shotSpeed).GetComponent<Rigidbody2D>().velocity = Vector2.left * shotSpeed;
            Shoot(4, 0, shotSpeed).GetComponent<Rigidbody2D>().velocity = Vector2.up * shotSpeed;
            Shoot(5, 0, shotSpeed).GetComponent<Rigidbody2D>().velocity = Vector2.right * shotSpeed;
            Shoot(6, 0, shotSpeed).GetComponent<Rigidbody2D>().velocity = Vector2.down * shotSpeed;
            yield return new WaitForSeconds(1.5f);
        }
    }

    public override void Reset()
    {
        if (initialized)
        {
            hasShot = false;
            explosionActive = false;
            ultimateAttackFinished = false;
            explosionHitbox.enabled = false;
            explosionSprite.enabled = false;
            ResetBrightness();
            StopAllCoroutines();
            base.Reset();
        }
    }

    protected override void OnTargetContact(Transform target)
    {
        TickDamage debuff = target.GetComponent<TickDamage>();
        if (debuff == null)
        {
            debuff = target.gameObject.AddComponent<TickDamage>();
            debuff.SetDefaults(stats.damage / 2, 1, 4);
        }
    }
}
