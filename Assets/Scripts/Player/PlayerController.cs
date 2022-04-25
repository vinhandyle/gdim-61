using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 playerDirection = Vector2.right;

    [Header("Movement Numbers")]
    public float speed;
    public float jumpHeight;
    public float airJumpHeight;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private float radius;
    public bool onGround;

    [Header("Wall Detection")]
    [SerializeField] private bool isTouchingWall;
    public Transform wallCheck;
    [SerializeField] private bool isSliding;
    public float slidingSpeed;

    [Header("Wall Jump")]
    [SerializeField] private bool isWallJumping;
    public float wallJumpTime;
    public float wallJumpHeight;
    public float wallJumpWidth;

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
    [SerializeField] private Vector2 facingDirections;
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool prematureEnd;
    public float dashCooldown;
    public float dashDuration;
    public float dashLength;
    private float dashTimeLeft;

    [Header("Combat")]
    [SerializeField] private BasicAttack basicAttack;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        airJumpsLeft = airJumpsMax;
        canDash = true;
    }

    private void Update()
    {
        // Set falling animation
        if (onGround)
        {
            airJumpsLeft = airJumpsMax;
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }
        else if (rb.velocity.y < 0 && !isDashing)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        }

        // Wall detection
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, isGround);

        isSliding = isTouchingWall && !onGround && (Controls.Left() || Controls.Right());

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
        }

        // Cannot move or begin another attack while an attack is in process
        if (!basicAttack.inProcess)
        {
            // Since dash and move both set velocity, have only one happen
            // Having both will cause move to override the dash
            if (!isDashing && !isWallJumping)
            {
                Move();
            }
            Jump();
            Dash();
            Attack();
        }
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

    #region Horizontal movement

    /// <summary>
    /// Defines the player's normal horizontal movement.
    /// </summary>
    private void Move()
    {
        int direction = 0;

        if (Controls.Left())
        {
            direction = -1;
            if (playerDirection.x > 0)
                FlipPlayer();
            playerDirection.x = -1;
        }
        else if (Controls.Right())
        {
            direction = 1;
            if (playerDirection.x < 0)
                FlipPlayer();
            playerDirection.x = 1;
        }

        // Set Idle or Walking animation
        if (direction != 0)
            anim.SetBool("Walking", true);
        else if (rb.velocity.y == 0)
            anim.SetBool("Walking", false);

        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    /// <summary>
    /// Flips the player to face the opposite direction
    /// </summary>
    void FlipPlayer()
    {
        Vector3 oppDirection = transform.localScale;
        oppDirection.x *= -1;
        transform.localScale = oppDirection;
    }

    #endregion

    #region Jump

    /// <summary>
    /// Defines the player's jump.
    /// </summary>
    private void Jump()
    {
        if (Controls.Jump()[1])
        {
            anim.SetBool("Jumping", true);

            // Cancel dash
            dashTimeLeft = 0;
            prematureEnd = true;

            // Regular jump
            if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                airJumpsLeft--;
            }
            // Wall jump
            else if (isSliding && !isWallJumping)
            {
                rb.velocity = new Vector2(-playerDirection.x * wallJumpWidth * speed, wallJumpHeight);
                playerDirection.x *= -1;
                FlipPlayer();
                StartCoroutine("WallJumpTimer");
            }
            // Air jump
            else if (airJumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, airJumpHeight);
                airJumpsLeft--;
            }
        }

        if (jumpCancelEnabled)
        {
            if (Controls.Jump()[2] && rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight / jumpReduction);
        }

        jumpPressed = Controls.Jump()[0];
    }

    /// <summary>
    /// Prevent moving out of wall jump for pre-specified time.
    /// </summary>
    IEnumerator WallJumpTimer()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        isWallJumping = false;
    }

    #endregion

    #region Dash

    /// <summary>
    /// Quickly moves the player forward a set distance.
    /// </summary>
    private void Dash()
    {
        // Determine the direction the dash will go
        // Direction is locked once dash is started
        if (!isDashing)
        {
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

            // Uncomment this if you want somewhat janky, 8-directional dash, rather than 4 directional
            // facingDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (Controls.Dash())
        {
            // Set player dash velocity
            if (canDash)
            {
                anim.SetBool("Dashing", true);
                rb.velocity = facingDirections * dashLength;
                dashTimeLeft = dashDuration; 
                canDash = false;
                isDashing = true;
                prematureEnd = false; // We keep this if we need to prematurely end the dash (i.e hit by enemy, hit a wall, etc)
                StartCoroutine(DashCooldown(dashCooldown));
            }
        }

        // The dashTime (or how long the dash will last) starts to tick down, when 0, the dash will stop
        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
        }
        // When the dash is over, reset player velocity to prevent excess movement
        else if (isDashing) 
        {
            if (prematureEnd)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = Vector2.zero;

            isDashing = false;
            anim.SetBool("Dashing", false);
        }
    }

    /// <summary>
    /// Start timer for dash cooldown.
    /// </summary>
    IEnumerator DashCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Must be on ground to reset dash
        while (!onGround)
            yield return null;

        canDash = true;
    }

    #endregion

    #region Basic Attack

    /// <summary>
    /// Start the attack for the player.
    /// </summary>
    public void Attack()
    {
        if(Controls.Attack())
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Attacking", true);
            basicAttack.Foreswing();
        }
    }

    /// <summary>
    /// Animation event for basic attack hit.
    /// </summary>
    public void AttackHit()
    {
        basicAttack.Hit();
    }

    /// <summary>
    /// Animation event for basic attack backswing.
    /// </summary>
    public void AttackBackswing()
    {
        basicAttack.Backswing();
    }

    /// <summary>
    /// Animation event for basic attack finish.
    /// </summary>
    public void AttackFinish()
    {
        basicAttack.Finish();
        anim.SetBool("Attacking", false);
    }

    #endregion
}