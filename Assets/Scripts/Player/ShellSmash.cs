using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSmash : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float dmg;
    [SerializeField] public bool groundCollision;

    [SerializeField] private List<Sprite> sprites;

    private BoxCollider2D hitbox;
    private SpriteRenderer spriteRender;
    public Collision2D collisionInfo;


    [SerializeField] public bool inProcess;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        hitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health enemy = collision.GetComponent<Health>();
            enemy.TakeDamage(dmg);
            
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            collisionInfo = collision;
            groundCollision = true;
        }
    }

    public void StartAttack()
    {
        inProcess = true;
    }
    public void Hit()
    {
        hitbox.enabled = true;
        spriteRender.sprite = sprites[0];
        Debug.Log("Hit");
    }

    public void EndHit()
    {
        hitbox.enabled = false;
        spriteRender.sprite = null;
    }

    public void FinishAttack()
    {
        inProcess = false;
    }
}
