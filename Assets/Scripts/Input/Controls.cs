using UnityEngine;

/// <summary>
/// The control scheme.
/// </summary>
public class Controls : MonoBehaviour
{
    /// <summary>
    /// Returns true while the user holds down any key mapped to the left direction.
    /// </summary>
    public static bool Left()
    {
        return Input.GetKey(KeyCode.A);
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the right direction.
    /// </summary>
    public static bool Right()
    {
        return Input.GetKey(KeyCode.D);
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the up direction.
    /// </summary>
    public static bool Up()
    {
        return Input.GetKey(KeyCode.W);
    }

    /// <summary>
    /// Returns true while the user holds down any key mapped to the down direction.
    /// </summary>
    public static bool Down()
    {
        return Input.GetKey(KeyCode.S);
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to the dash ability.
    /// </summary>
    public static bool Dash()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    /// <summary>
    /// Returns true if the user presses any key mapped to attack
    /// </summary>
    /// <returns></returns>
    public static bool Attack()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    /// <summary>
    /// Returns true if the user presses the assigned to bring up the level selection.
    /// </summary>
    public static bool SelectLevel()
    {
        return Input.GetKeyDown(KeyCode.P);
    }
}
