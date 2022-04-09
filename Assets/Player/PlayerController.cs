using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [Header("Movement Numbers")]
    public float speed;
    public float jumpHeight;
    public float airJumpHeight;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private float radius;
    public bool onGround;

    [Header("Short Hops")]
    [SerializeField] private bool jumpCancelEnabled;
    public float jumpReduction;

    [Header("Air Jumps")]
    public int airJumpsLeft;
    public int airJumpsMax;

    [Header("Freefall")]
    [SerializeField] private bool usingAccelFall;
    [SerializeField] private bool jumpPressed;
    public float fallMultiplier = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        airJumpsLeft = airJumpsMax;
    }

    private void Update()
    {
        if (onGround)
            airJumpsLeft = airJumpsMax;

        // TODO: Add dash
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, radius, isGround);

        if (usingAccelFall)
        {
            if (rb.velocity.y < 0 || !jumpPressed)
                rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;

            if (rb.velocity.y < 4 && rb.velocity.y > 0 && jumpPressed && airJumpsLeft > 0)
                rb.velocity += 2 * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
    }

    /// <summary>
    /// Defines the player's normal horizontal movement.
    /// </summary>
    private void Move()
    {
        int direction = 0;

        if (Controls.Left())
        {
            direction = -1;
            sprite.flipX = true;
        }
        else if (Controls.Right())
        {
            direction = 1;
            sprite.flipX = false;
        }

        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    /// <summary>
    /// Defines the player's jump.
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                airJumpsLeft--;
            }
            else if (airJumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, airJumpHeight);
                airJumpsLeft--;
            }
        }

        if (jumpCancelEnabled)
        {
            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight / jumpReduction);
        }

        jumpPressed = Input.GetButton("Jump");
    }
}
