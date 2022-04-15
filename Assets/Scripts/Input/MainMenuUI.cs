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
        SceneController.Instance.UnloadScene("Main Menu");
        SceneController.Instance.LoadScene("Level 1");
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
