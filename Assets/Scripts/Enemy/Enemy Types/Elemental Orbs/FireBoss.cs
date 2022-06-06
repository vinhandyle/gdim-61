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

    private float linger;
    private float stuckTimer;
    private Vector3 offset = new Vector3(0, 0, 0);

    protected override void Awake()
    {
        base.Awake();
        initialColor = GetComponent<SpriteRenderer>().material.color;
    }

    private void OnEnable()
    {
        AudioController.Instance.PlayTrack(2);
    }

    protected override void AI()
    {
        CheckAggro();
        CheckShootRange();

        if (currentTarget != null)
        {
            PointAtTarget(rotator, currentTarget);

            float inertia = 2f;
            float speedMult = 0.02f;
            linger += Time.deltaTime;
            if((int) linger % 8 == 0)
            {
                offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            }
            Vector2 direction = currentTarget.position + offset - transform.position;
            direction.Normalize();

            if (direction.sqrMagnitude > 50)
            {
                speedMult = 0.05f;
            }
            else
            {
                speedMult = 0.02f;
            }

            direction *= stats.speed * speedMult;
            rb.velocity = rb.velocity * (inertia - 1) + direction / inertia;
        }
        else
        {
            rb.velocity *= 0.9f;
        }

        // Don't let the boss get stuck and look like an idiot
        if(rb.velocity.x < 0.5f && rb.velocity.y < 0.5f)
        {
            stuckTimer += Time.deltaTime;
            if(stuckTimer > 2)
            {
                rb.velocity = offset / 1.5f;
                stuckTimer = 0;
            }
        }

        // Basic fireball attack
        if (!hasShot && currentTarget)
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

    private void OnDisable()
    {
        if (SceneController.Instance.currentScene == "Level 1")
            AudioController.Instance.PlayTrack(1);
    }

    #region Fireball Attack

    private IEnumerator FireballAttack()
    {
        Shoot(0, 0, shotSpeed);
        Shoot(1, 0, shotSpeed);
        Shoot(2, 0, shotSpeed);

        hasShot = true;
        yield return new WaitForSeconds(5);
        hasShot = false;
    }

    #endregion

    #region Explosion Attack

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

    #endregion

    #region Ultimate Attack

    private IEnumerator UltimateAttack()
    {
        ultimateAttackFinished = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 3; j <= 10; ++j)
            {
                Shoot(j, 0, shotSpeed);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    #endregion

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
