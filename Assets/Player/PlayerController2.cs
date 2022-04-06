using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : Controller
{
    protected override void MoveIM()
    {
        int direction = 0;
        if (Input.GetKey(KeyCode.A))
            direction = -1;
        else if (Input.GetKey(KeyCode.D))
            direction = 1;
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        sprite.flipX = direction == 0 ? sprite.flipX : direction < 0;
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
                rb.velocity = new Vector2(rb.velocity.x, jumpheight);
                extraJumps--;
            }
            else if (extraJumps > 0 && !onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, doublejumpheight);
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
