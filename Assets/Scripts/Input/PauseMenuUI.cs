using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines UI elements in the pause menu.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject controlsCanvas;

    [SerializeField] private GameObject rebindPanel;
    [SerializeField] private GameObject backButton;

    private void Update()
    {
        // Open or close level selection UI
        if (
            GameStateManager.Instance.currentState == GameStateManager.GameState.RUNNING
            && canvas != null
            && (Controls.Instance.Pause() || GameStateManager.Instance.mainMenuPaused)
            )
        {
            canvas.SetActive(!canvas.activeSelf);
            GameStateManager.Instance.TogglePause();
        }

        if (rebindPanel != null && backButton != null) backButton.SetActive(!rebindPanel.activeSelf); 
    }

    /// <summary>
    /// Close the pause menu and resume gameplay.
    /// </summary>
    public void Resume()
    {
        canvas.SetActive(false);
        GameStateManager.Instance.TogglePause();
    }

    /// <summary>
    /// Return to the main menu.
    /// </summary>
    public void Exit()
    {
        canvas.SetActive(false);
        if (SceneController.Instance.currentScene != "Main Menu")
        {
            SceneController.Instance.UnloadCurrentScene();
            SceneController.Instance.LoadScene("Main Menu");
        }
        GameStateManager.Instance.TogglePause();
    }

    /// <summary>
    /// Switch to the controls menu.
    /// </summary>
    public void Rebind()
    {
        controlsCanvas.SetActive(true);
        canvas.SetActive(false);
    }

    /// <summary>
    /// Return to the pause menu.
    /// </summary>
    public void Back()
    {
        canvas.SetActive(true);
        controlsCanvas.SetActive(false);
    }
}
