using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingDebris : MonoBehaviour
{
    // Need to add waypoint where debris will reset position, one debris beaks on impact
    [SerializeField] private float debrisSpeed;
    private Transform initialTransform;
    private Vector2 initialPosition;
    private Rigidbody2D rig;
    private Vector2 screen;


    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        initialTransform = this.GetComponent<Transform>();
        initialPosition = new Vector2(initialTransform.position.x, initialTransform.position.y);
        rig.velocity = new Vector2(0, -debrisSpeed);
        screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -screen.y * 1.5)
        {
            Debug.Log(initialPosition.y);
            transform.position = new Vector2(initialPosition.x, initialPosition.y);
            Debug.Log("ur mom");
        }
    }

    
}
