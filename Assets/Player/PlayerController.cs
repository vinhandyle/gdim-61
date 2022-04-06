using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    protected override void MoveIM()
    { 
        moveinput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveinput * speed, rb.velocity.y);
        sprite.flipX = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) ? Input.GetKey(KeyCode.A) : sprite.flipX;
    }

    protected override void MoveIS()
    {
        throw new System.NotImplementedException();
    }

    protected override void JumpIM()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector2.up * jumpheight, ForceMode2D.Impulse);
                extraJumps--;
            }
            else if (extraJumps > 0 && !onGround)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector2.up * doublejumpheight, ForceMode2D.Impulse);
                extraJumps--;
            }
        }

        if (jumpCancelEnabled && Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, jumpheight / jumpReduction);

        jumpPressed = Input.GetButton("Jump");
    }

    protected override void JumpIS()
    {
        throw new System.NotImplementedException();
    }
}
