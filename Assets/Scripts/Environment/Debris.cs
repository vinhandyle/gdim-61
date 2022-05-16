using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    // Need to add waypoint where debris will reset position, one debris beaks on impact
    [SerializeField] private float debrisSpeed;
    private Rigidbody2D rig;
    private Vector2 screen;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private bool isTouchingGround;
    [SerializeField] private float radius;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(0, -debrisSpeed);
        screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(transform.position, radius, isGround);

        if (transform.position.y < -screen.y * 1.5)
        {
            Destroy(gameObject);
        }

        if (isTouchingGround)
        {
            Destroy(gameObject);
        }
    }
}
