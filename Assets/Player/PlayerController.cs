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

    [Header("Dashing")]
    private Vector2 facingDirections;
    bool isDashing;
    bool prematureEnd;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    float dashTime;
    float dashCooldownTime;
    [SerializeField] float dashLength;

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
        Dash();
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

        // Siince dash and move both set velocity
        // Have only one happen, having both will cause move to override the dash
        if(!isDashing)
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        }
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

    /// <summary>
    /// Quickly moves the player forward a set distance
    /// </summary>
    private void Dash()
    {
        // Determnine the direction the dash will go
        if (Controls.Left())
        {
            facingDirections.x = -1;
            facingDirections.y = 0;
        }
        else if (Controls.Right())
        {
            facingDirections.x = 1;
            facingDirections.y = 0;
        }
        else if (Controls.Up())
        {
            facingDirections.x = 0;
            facingDirections.y = 1;
        }
        else if (Controls.Down())
        {
            facingDirections.x = 0;
            facingDirections.y = -1;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCooldownTime <= 0)
            {
                dashTime = dashDuration; // Set the current dashTime counter to be how long the dash will last
                dashCooldownTime = dashCooldown; // Set the dashCooldownTime to how long the cooldown will be 
                isDashing = true; // Is the player currently dashing right now?
                prematureEnd = false; // We keep this if we need to prematurely end the dash (i.e hit by enemy, hit a wall, etc)
                StartCoroutine(DashCooldown(dashCooldown)); // Start the Couroutine to end teh dash at the specified time
            }
        }
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime; // The dashTime (or how long the dash will last) starts to tick down, when 0, the dash will stop

            // Uncomment this if you want somewhat janky, 8-directional dash, rather than 4 directional
            //facingDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            rb.velocity = facingDirections * dashLength; // Set the velocity based on dashLength and the direcion the player is facing
        }

        if (isDashing && dashTime <= 0 && !prematureEnd) // The dash has now ended
        {
            rb.velocity = Vector2.zero; // Set the velocity to zero so the player isn't flung in some direction
            isDashing = false; // The player is no longer dashing
        }
    }
    IEnumerator DashCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dashCooldownTime = 0;
    }
}
