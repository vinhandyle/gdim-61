using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    // Need to add waypoint where debris will reset position, one debris beaks on impact
    [SerializeField] private float debrisSpeed;
    [SerializeField] private bool isPlatform;
    private Rigidbody2D rig;
    private Vector2 screen;

    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(0, -debrisSpeed);
        screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -screen.y * 1.5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health victim = collision.gameObject.GetComponent<Health>();
        if (!isPlatform && victim != null)
        {
            // TODO: Write this more efficiently
            if (victim.GetComponent<Ogre>() || victim.GetComponent<PlayerController>())
            {
                victim.Die();
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
