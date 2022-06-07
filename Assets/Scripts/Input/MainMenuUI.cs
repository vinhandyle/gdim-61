using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines UI elements in the main menu.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    /// <summary>
    /// Load the first level. (Modify if we want to have a story scene)
    /// </summary>
    public void StartGame()
    {

        Controls.Instance.dashUnlocked = false;
        Controls.Instance.slamUnlocked = false;
        Controls.Instance.grappleUnlocked = false;
        Controls.Instance.spAttackUnlocked = false;
        SceneController.Instance.UnloadScene("Main Menu");
        SceneController.Instance.LoadScene("Level 1");
    }

    /// <summary>
    /// Open the pause menu (settings).
    /// </summary>
    public void OpenSettings()
    {
        GameStateManager.Instance.mainMenuPaused = true;
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
