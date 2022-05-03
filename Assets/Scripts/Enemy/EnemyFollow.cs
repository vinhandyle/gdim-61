using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask isPlayer;

    public float radius;
    public float aggroSpeed;
    public float wanderSpeed;
    private float wanderDirection = 1;
    [SerializeField] private bool preventFall;
    [SerializeField] private float cushionTime;


    [Header("Player Checking")]
    [SerializeField] private bool playerInArea;
    [SerializeField] private bool isFollowing;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform player;
    private float playerDirection;

    [Header("Raycasts")]
    [SerializeField] private RaycastHit2D ifGround;
    [SerializeField] private float distance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    
    void Update()
    {
        playerInArea = Physics2D.OverlapCircle(playerCheck.position, radius, isPlayer);

        if (playerInArea == true)
        {
            if (ifGround.collider == null)
            {
                isFollowing = false;
            }
            else
            {
                isFollowing = true;
                GetPlayerDirection();
            }
            
        }

        else
        {
            isFollowing = false;
        }
        

        CheckGround();

        MoveAI();

    }

    private void MoveAI()
    {
        if (isFollowing == true && preventFall == false)
        {
            rb.velocity = new Vector2(aggroSpeed * playerDirection, rb.velocity.y);

            if (ifGround.collider == null && isFollowing == true)
            {
                StartCoroutine("PreventFall");
            }
        }
        else
        {
            rb.velocity = new Vector2(wanderSpeed * wanderDirection, rb.velocity.y);
        }
    }

    private void CheckGround()
    {
        ifGround = Physics2D.Raycast(groundCheck.position, Vector2.down, distance, isGround);

        if (ifGround.collider == null)
        {
            FlipSprite();
            wanderDirection *= -1;
        }
    }

    void FlipSprite()
    {
        Vector3 oppDirection = transform.localScale;
        oppDirection.x *= -1;
        transform.localScale = oppDirection;
    }

    private void GetPlayerDirection()
    {
        if (player.transform.position.x > rb.transform.position.x)
        {
            playerDirection = 1;
        }

        else
        {
            playerDirection = -1;
        }
    }

    IEnumerator PreventFall()
    {
        
        preventFall = true;

        yield return new WaitForSeconds(cushionTime);

        preventFall = false;
    }
}
