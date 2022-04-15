using System.Collections;
using UnityEngine;

/// <summary>
/// Defines UI elements in the level selector.
/// </summary>
public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private void Update()
    {
        // Open or close level selection UI
        if (
            GameStateManager.Instance.currentState != GameStateManager.GameState.PREGAME 
            && canvas != null
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
