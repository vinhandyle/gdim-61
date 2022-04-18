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
    private SpriteRenderer sprite;
    private Vector2 playerDirection;
    private bool facingRight = true;
    private float input;

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
    [SerializeField] bool isTouchingWall;
    public Transform wallCheck;
    [SerializeField] bool isSliding;
    public float slidingSpeed;

    [Header("Wall Jump")]
    [SerializeField] bool isWalljumping;
    public float wallJumptime;
    public float walljumpHeight;

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
    [SerializeField] private double damage;
    [SerializeField] private float attackDuration;
    [SerializeField] private Transform attackBox1;
    [SerializeField] private Transform attackBox2;
    [SerializeField] private Transform attackBox3;
    private bool canMove;
    private bool isAttacking;
    private int attackFrameCounter;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        airJumpsLeft = airJumpsMax;
        canDash = true;
        canMove = true;
        attackFrameCounter = 0;
    }

    private void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, isGround);

        if (onGround)
            airJumpsLeft = airJumpsMax;

        if (isTouchingWall == true && onGround == false && input != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
        }

        if (canMove)
        {
            // Since dash and move both set velocity
            // Have only one happen, having both will cause move to override the dash
            if (!isDashing)
            {
                Move();
            }
            Jump();

            if (isWalljumping == true)
            {
                rb.velocity = new Vector2(-rb.velocity.x, walljumpHeight);
                StartCoroutine("WallJumpTimer");
            }

            Dash();
        }

        Attack();
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

        if (facingRight == false && input > 0)
        {
            FlipSprite();
        }
        else if (facingRight == true && input < 0)
        {
            FlipSprite();
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
            playerDirection.x = -1;
        }
        else if (Controls.Right())
        {
            direction = 1;
            playerDirection.x = 1;
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

            else if (isSliding == true)
            {
                isWalljumping = true;
                
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
        else if (isDashing && !prematureEnd) 
        {
            rb.velocity = Vector2.zero;
            isDashing = false;
        }
    }

    /// <summary>
    /// Start timer for dash cooldown.
    /// </summary>
    IEnumerator DashCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canDash = true;
    }

    /// <summary>
    /// Flips the player to face the opposite direction
    /// </summary>
    void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 oppDirection = transform.localScale;
        oppDirection.x *= -1;
        transform.localScale = oppDirection;
    }
    /// <summary>
    /// Start the attack for the player
    /// </summary>
    public void Attack()
    {
        if(Controls.Attack() && !isAttacking)
        {
            canMove = false;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            StartCoroutine(StartAttackHitbox());
            StartCoroutine(EndAttack());
        }
    }

    /// <summary>
    /// Activate the corresponding hurtbox based on player direction
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAttackHitbox()
    {
        yield return new WaitForSeconds(0.05f);
        if(playerDirection.x == -1)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackBox2.position, 0.5f, LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyInfo = enemy.GetComponent<Enemy>();
                enemyInfo.DamageEnemy(damage);
                Debug.Log("hit enemy with box3");
            }
        }
        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackBox3.position, 0.5f, LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyInfo = enemy.GetComponent<Enemy>();
                enemyInfo.DamageEnemy(damage);
                Debug.Log("hit enemy with box3");
            }
        }
    }

    /// <summary>
    /// End the attack cycle
    /// </summary>
    /// <returns></returns>
    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(attackDuration - 0.22f);
        canMove = true;
        isAttacking = false;
    }

    /// <summary>
    /// Controls how long the wall jump lasts for
    /// </summary>
    /// <returns></returns>
    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(wallJumptime);
        isWalljumping = false;
    }

    // Visualize what the attack hitboxes are
    private void OnDrawGizmosSelected()
    {
        if(attackBox1 != null)
        {
            Gizmos.DrawWireSphere(attackBox1.position, 0.5f);
        }
        if (attackBox2 != null)
        {
            Gizmos.DrawWireSphere(attackBox2.position, 0.5f);
        }
        if (attackBox3 != null)
        {
            Gizmos.DrawWireSphere(attackBox3.position, 0.5f);
        }
    }
}
