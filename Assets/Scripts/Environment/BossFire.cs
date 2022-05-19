using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;

    void Start()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 4) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyFire(collision.gameObject);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyFire(collision.gameObject);
        }
    }

    private void ApplyFire(GameObject target)
    {
        TickDamage debuff = target.GetComponent<TickDamage>();
        if (debuff == null)
        {
            debuff = target.AddComponent<TickDamage>();
            debuff.SetDefaults(damage / 2, 1, 3);
        }
        else
        {
            debuff.Reapply();
        }
    }
}
