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
    private PlayerImmunity immunity;

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
    [SerializeField] private Vector2 playerDirection = Vector2.right;
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool prematureEnd;
    [SerializeField] private float dashDamage;
    [SerializeField] private DashAttack dashAttack;
    public float dashCooldown;
    public float dashDuration;
    public float dashLength;
    private float dashTimeLeft;

    [Header("Combat")]
    [SerializeField] private MeleeAttack basicAttack;
    [SerializeField] private MeleeAttack specialAttack;

    [Header("ShellSmash")]
    [SerializeField] private float stallTime;
    [SerializeField] private float slamForce;
    [SerializeField] private bool isShellSmashing;
    [SerializeField] private bool canShellSmash;
    [SerializeField] private float smashCooldown;


    // For modifying player velocity outside of this class
    private bool overrideMovement;

    

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        immunity = GetComponent<PlayerImmunity>();
        airJumpsLeft = airJumpsMax;
        canDash = true;
        overrideMovement = false;
    }

    private void Update()
    {
        // Set falling animation
        if (onGround)
        {
            airJumpsLeft = airJumpsMax;
            anim.SetBool("Falling", false);
            anim.SetInteger("Slamming", 0);
        }
        else if (rb.velocity.y <= 0 && !isDashing)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        }

        // Wall slide
        isSliding = isTouchingWall && !onGround && (Controls.Instance.Left() || Controls.Instance.Right());

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
        }

        // Cannot move or begin another attack while an attack is in process
        if (!(basicAttack.inProcess || specialAttack.inProcess))
        {
            // Since dash and move both set velocity, have only one happen
            // Having both will cause move to override the dash
            if (!isDashing && !isWallJumping && !isShellSmashing)
            {
                Move();
            }

            if (!onGround)
            {
                ShellSmash();
            }
            else
            {
                isShellSmashing = false;               
            }
            
            Jump();
            Dash();
            Attack();

            if (!basicAttack.inProcess) SpecialAttack();
        }
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, radius, isGround);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, isGround);

        if (usingAccelFall && rb.bodyType != RigidbodyType2D.Static)
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

        if (Controls.Instance.Left())
        {
            direction = -1;
            if (playerDirection.x >= 0)
                FlipPlayer();
            playerDirection.x = -1;
        }
        else if (Controls.Instance.Right())
        {
            direction = 1;
            if (playerDirection.x <= 0)
                FlipPlayer();
            playerDirection.x = 1;
        }

        // Set Idle or Walking animation
        if (direction != 0)
            anim.SetBool("Walking", true);
        else if (rb.velocity.y == 0)
            anim.SetBool("Walking", false);

        // We add this check so that if we want to change player velocity, 
        // this line does not instantly set the x velocity to 0 on the next frame
        if(!overrideMovement)
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        }

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
        if (Controls.Instance.Jump()[1])
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Falling", false);
            Controls.Instance.asyncInputs.receivedJump[0] = true;

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
            // Cut the jump short
            if (Controls.Instance.Jump()[2])
            {
                Controls.Instance.asyncInputs.receivedJump[1] = true;
                if (rb.velocity.y > 0)
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight / jumpReduction);
            }
        }

        jumpPressed = Controls.Instance.Jump()[0];
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
        if (!isDashing && !isWallJumping)
        {
            if (Controls.Instance.Left()) playerDirection = Vector2.left;
            else if (Controls.Instance.Right()) playerDirection = Vector2.right;
            else if (Controls.Instance.Up()) playerDirection = Vector2.up;
            else if (Controls.Instance.Down()) playerDirection = Vector2.down;

            // Uncomment this if you want somewhat janky, 8-directional dash, rather than 4 directional
            // facingDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (Controls.Instance.Dash())
        {
            // Set player dash velocity
            if (canDash)
            {
                immunity.EnablePlayerImmunity();
                dashAttack.Hit();
                anim.SetBool("Dashing", true);
                Controls.Instance.asyncInputs.receivedDash = true;

                rb.velocity = playerDirection * dashLength;
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
            immunity.DisablePlayerImmunity();
            dashAttack.Finish();
            anim.SetBool("Dashing", false);
        }
    }

    /// <summary>
    /// Returns whether or not the player is dashing
    /// </summary>
    public bool IsDashing()
    {
        return isDashing;
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

    #region Shell Smash

    /// <summary>
    /// Starts the shell smash ability.
    /// </summary>
    private void ShellSmash()
    {
        if (Controls.Instance.GroundPound())
        {
            if (canShellSmash)
            {
                isShellSmashing = true;
                anim.SetInteger("Slamming", 1);
            }
        }
    }

    /// <summary>
    /// Section of the ability where the player lingers in the air.
    /// </summary>
    public void AirStall()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    /// <summary>
    /// Section of the ability where the player slams down.
    /// </summary>
    public void Slam()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);
        anim.SetInteger("Slamming", 2);

        StartCoroutine("ShellSmashCooldown");
    }

    private IEnumerator ShellSmashCooldown()
    {
        canShellSmash = false;

        yield return new WaitForSeconds(smashCooldown);

        canShellSmash = true;
    }

    #endregion

    #region Basic Attack

    /// <summary>
    /// Start the player's basic attack.
    /// </summary>
    public void Attack()
    {
        if(Controls.Instance.Attack())
        {
            Controls.Instance.asyncInputs.receivedAttack = true;
            rb.velocity = Vector2.zero;
            anim.SetInteger("Attacking", 1);
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
        anim.SetInteger("Attacking", 0);
    }

    #endregion

    #region Special Attack

    /// <summary>
    /// Start the player's special attack.
    /// </summary>
    public void SpecialAttack()
    {
        if (Controls.Instance.SpecialAttack())
        {
            rb.velocity = Vector2.zero;
            anim.SetInteger("Attacking", 2);
            specialAttack.Foreswing();
        }
    }

    /// <summary>
    /// Animation event for special attack hit.
    /// </summary>
    public void SpecialAttackHit()
    {
        specialAttack.Hit();
    }

    /// <summary>
    /// Animation event for special attack backswing.
    /// </summary>
    public void SpecialAttackBackswing()
    {
        specialAttack.Backswing();
    }

    /// <summary>
    /// Animation event for special attack finish.
    /// </summary>
    public void SpecialAttackFinish()
    {
        specialAttack.Finish();
        anim.SetInteger("Attacking", 0);
    }

    #endregion

    #region Misc

    /// <summary>
    /// Add and launch a player by some directional force
    /// This will prevent the Move() function from resetting the player's 
    /// x velocity for the time specified (in seconds).
    /// </summary>
    public void AddForce(Vector2 force, float seconds)
    {
        overrideMovement = true;
        //rb.AddForce(force);

        rb.velocity += force;

        StartCoroutine(ReturnMovement(seconds));
    }

    private IEnumerator ReturnMovement(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        overrideMovement = false;
    }

    #endregion 
}