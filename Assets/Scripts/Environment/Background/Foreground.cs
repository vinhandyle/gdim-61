using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    [SerializeField] private bool translucent;
    [SerializeField] private float range;
    [SerializeField] private float minAlpha;

    private Transform player;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Color c = sprite.material.color;
        float a = 1;

        if (translucent)
        {
            float dist = Mathf.Abs(transform.position.x - player.position.x);
            a = dist < range ? 0 : dist / GetComponent<BoxCollider2D>().size.x;
            a = Mathf.Max(a, minAlpha);
        }
        sprite.material.color = new Color(c.r, c.g, c.b, a);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            translucent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
            translucent = false;
        }
    }
}
