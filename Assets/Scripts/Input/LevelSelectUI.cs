using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines UI elements in the level selector.
/// </summary>
public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private List<GameObject> pauseMenu;

    private void Update()
    {
        // Open or close level selection UI (while not in pause menu)
        if (
            GameStateManager.Instance.currentState != GameStateManager.GameState.PREGAME 
            && canvas != null
            && pauseMenu.TrueForAll(g => !g.activeSelf)
            && Controls.SelectLevel()
            )
        {
            canvas.SetActive(!canvas.activeSelf);
            GameStateManager.Instance.TogglePause();
        }
    }

    /// <summary>
    /// Load the level associated with this button.
    /// </summary>
    public void LoadLevel()
    {
        string level = GetComponentInChildren<UnityEngine.UI.Text>().text;

        SceneController.Instance.UnloadCurrentScene();
        SceneController.Instance.LoadScene(level);
        GameStateManager.Instance.TogglePause();
        canvas.SetActive(false);
    }
}
