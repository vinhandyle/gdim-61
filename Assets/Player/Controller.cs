using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    [Header("Debug Toggle")]
    public bool usingInputSystem;
    public bool usingAccelFall;

    [Header("Movement Numbers")]
    public float speed;
    public float jumpheight;
    public float doublejumpheight;
    public float moveinput;

    [Header("Ground Detection")]
    public bool onGround;
    public Transform groundCheck;
    public float radias;
    public LayerMask isGround;

    [Header("Short Hops")]
    public bool jumpCancelEnabled;
    public float jumpReduction;

    [Header("Midair Jumps")]
    public int extraJumps;
    public int extraJumpsNumber;

    [Header("Freefall")]
    public float fallMultiplier = 2.5f;
    public bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsNumber;
    }

    protected virtual void Update()
    {
        if (onGround)
            extraJumps = extraJumpsNumber;

        Jump();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        onGround = Physics2D.OverlapCircle(groundCheck.position, radias, isGround);

        if (usingAccelFall)
        {
            if (rb.velocity.y < 0 || !jumpPressed)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

            if (rb.velocity.y < 4 && rb.velocity.y > 0 && jumpPressed && extraJumps > 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * 2 * Time.fixedDeltaTime;
        }
    }

    protected void Move()
    {
        if (usingInputSystem)
            MoveIS();
        else
            MoveIM();
    }

    protected void Jump()
    {
        if (usingInputSystem)
            JumpIS();
        else
            JumpIM();
    }

    protected abstract void MoveIM();
    protected abstract void MoveIS();
    protected abstract void JumpIM();
    protected abstract void JumpIS();
}
