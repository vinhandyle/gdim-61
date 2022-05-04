using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The control scheme.
/// </summary>
public class Controls : Singleton<Controls>
{
    public ControlsInputClass inputs;
    public ControlsAsyncInputClass asyncInputs;
    public ControlsHeldInputClass heldInputs;

    private void Update()
    {
        // Reload so that rebinded controls work at runtime
        if (GameStateManager.Instance.currentState == GameStateManager.GameState.PAUSED)
        {
            GetComponent<PlayerInput>().DeactivateInput();
        }
        else
        {
            GetComponent<PlayerInput>().ActivateInput();
        }

        CheckAsyncInputsReceived();
    }

    /// <summary>
    /// Checks whether player controller received any of the async inputs.
    /// Needed for no input holding implementation since context triggers
    ///     don't last long enough for the player controller to read them.
    /// </summary>
    private void CheckAsyncInputsReceived()
    {
        if (inputs.jump[1]) inputs.jump[1] = !asyncInputs.receivedJump[0] || heldInputs.canHoldJump;
        if (inputs.jump[2]) inputs.jump[2] = !asyncInputs.receivedJump[1];

        if (inputs.attack) inputs.attack = !asyncInputs.receivedAttack || heldInputs.canHoldAttack;
        if (inputs.dash) inputs.dash = !asyncInputs.receivedDash || heldInputs.canHoldDash;
        if (inputs.slam) inputs.slam = !asyncInputs.receivedSlam || heldInputs.canHoldSlam;
        if (inputs.grapple) inputs.grapple = !asyncInputs.receivedGrapple || heldInputs.canHoldGrapple;
        if (inputs.spAttack) inputs.spAttack = !asyncInputs.receivedSpAttack || heldInputs.canHoldSpAttack;
    }

    #region Unity Events for Input System

    // Left, Right, Up, Down only support input holding 
    // Jump, Attack, and the 4 abilities also support no input holding

    public void Left(InputAction.CallbackContext context)
    {
        if (context.started) inputs.left = true;
        if (context.canceled) inputs.left = false;
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.started) inputs.right = true;
        if (context.canceled) inputs.right = false;
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started) inputs.up = true;
        if (context.canceled) inputs.up = false;
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.started) inputs.down = true;
        if (context.canceled) inputs.down = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.jump[0] = true;
            inputs.jump[1] = true;
            asyncInputs.receivedJump[0] = false;
        }

        if (context.canceled)
        {
            inputs.jump[0] = false;
            inputs.jump[1] = false;
            inputs.jump[2] = true;
            asyncInputs.receivedJump[1] = false;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.attack = true;
            asyncInputs.receivedAttack = false;
        }

        if (context.canceled) inputs.attack = false;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.dash = true;
            asyncInputs.receivedDash = false;
        }

        if (context.canceled) inputs.dash = false;
    }

    public void Slam(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.slam = true;
            asyncInputs.receivedSlam = false;
        }

        if (context.canceled) inputs.slam = false;
    }

    public void Grapple(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.grapple = true;
            asyncInputs.receivedGrapple = false;
        }

        if (context.canceled) inputs.grapple = false;
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inputs.spAttack = true;
            asyncInputs.receivedSpAttack = false;
        }

        if (context.canceled) inputs.spAttack = false;
    }

    #endregion

    #region Player Controls

    /// <summary>
    /// Returns true while the user holds down any key mapped to the left direction.
    /// </summary>
    public bool Left()
    {
        return inputs.left;
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the right direction.
    /// </summary>
    public bool Right()
    {
        return inputs.right;
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the up direction.
    /// </summary>
    public bool Up()
    {
        return inputs.up;
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the down direction.
    /// </summary>
    public bool Down()
    {
        return inputs.down;
    }

    /// <summary>
    /// Returns true when the user lets go of any of the movement keys
    /// </summary>
    public bool MovementKeyUp()
    {
        return Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D);
    }

    /// <summary>
    /// <para>Returns a list of the statuses of the following three inputs by index: </para>
    /// <br>0: True while the player is holding down the mapped key for jump</br>
    /// <br>1: True if the player pressed the mapped key for jump</br>
    /// <br>2: True if the player released the mapped key for jump</br>
    /// </summary>
    public bool[] Jump()
    {
        return inputs.jump;
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to attack.
    /// </summary>
    public bool Attack()
    {
        return inputs.attack;
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to the dash ability.
    /// </summary>
    public bool Dash()
    {
        return inputs.dash;
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to the slam ability.
    /// </summary>
    public bool GroundPound()
    {
        return inputs.slam;
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to the grapple ability.
    /// </summary>
    public bool Grapple()
    {
        return inputs.grapple;
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to special attack.
    /// </summary>
    public bool SpecialAttack()
    {
        return inputs.spAttack;
    }
    #endregion

    #region Menus

    /// <summary>
    /// Returns true if the user presses any key mapped to pause.
    /// </summary>
    public bool Pause()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    /// <summary>
    /// Returns true if the user presses the assigned to bring up the level selection.
    /// </summary>
    public bool SelectLevel()
    {
        return Input.GetKeyDown(KeyCode.P);
    }

    #endregion
}

/// <summary>
/// Collapsible section for inputs received from Unity Events.
/// </summary>
[System.Serializable]
public class ControlsInputClass
{
    public bool left, right, up, down, attack, dash, slam, grapple, spAttack;
    public bool[] jump = new bool[3];
}

/// <summary>
/// Collapsible section for inputs received by the player controller.
/// </summary>
[System.Serializable]
public class ControlsAsyncInputClass
{
    public bool[] receivedJump = new bool[2];
    public bool receivedAttack, receivedDash, receivedSlam, receivedGrapple, receivedSpAttack;
}

/// <summary>
/// Collapsible section for held input configuration.
/// </summary>
[System.Serializable]
public class ControlsHeldInputClass
{
    public bool canHoldJump, canHoldAttack, canHoldDash, canHoldSlam, canHoldGrapple, canHoldSpAttack;
}