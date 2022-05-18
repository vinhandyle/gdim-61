using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines UI elements in the debug menu(s).
/// </summary>
public class DebugMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private List<GameObject> debugMenus;
    [SerializeField] private List<GameObject> pauseMenus;

    [Header("Player")]
    [SerializeField] private bool inLevel;
    [SerializeField] private Transform player;
    public int checkpointNum;

    private void Update()
    {
        // Open or close level selection UI (while not in pause menu)
        if (
            GameStateManager.Instance.currentState != GameStateManager.GameState.PREGAME
            && canvas != null
            && pauseMenus.TrueForAll(menu => !menu.activeSelf)
            && Controls.Instance.OpenDebug()
            )
        {
            canvas.SetActive(!canvas.activeSelf);
            GameStateManager.Instance.TogglePause();
        }

        inLevel = new List<string>() { "Boot", "Main Menu" }.TrueForAll(scene => SceneController.Instance.currentScene != scene);

        if (inLevel)
        {
            player = FindObjectOfType<PlayerController>()?.transform;
        }
    }

    /// <summary>
    /// Switch the active debug menu.
    /// </summary>
    public void Switch()
    {
        debugMenus.ForEach(menu => menu.SetActive(!menu.activeSelf));
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

    /// <summary>
    /// Sends the player to previous, current, or next checkpoint.
    /// The current checkpoint is the last one touched by the player.
    /// </summary>
    public void LoadCheckpoint(int increment)
    {
        if (inLevel)
        {
            List<Checkpoint> checkpoints = FindObjectOfType<CheckpointArray>().checkpoints;

            if (checkpointNum + increment >= checkpoints.Count || checkpointNum + increment < 0)
            {
                increment = 0;
            }
            checkpointNum += increment;

            Vector3 checkpoint = checkpoints[checkpointNum].transform.position;
            player.position = new Vector3(checkpoint.x, checkpoint.y, player.position.z);
        }

        GameStateManager.Instance.TogglePause();
        canvas.SetActive(false);
    }

    /// <summary>
    /// Toggle the player being able to die.
    /// </summary>
    public void ToggleImmortality()
    {
        if (inLevel)
        {
            player.GetComponent<Health>().canDie = !GetComponent<Toggle>().isOn;
        }
    }

    /// <summary>
    /// Toggle the player being able to contact enemies and their projectiles.
    /// </summary>
    public void ToggleIntangibility()
    {
        if (inLevel)
        {
            player.GetComponent<PlayerImmunity>().ToggleIntangibility(GetComponent<Toggle>().isOn);
        }
    }
}
