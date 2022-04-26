using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The statues at the end of each level.
/// </summary>
public class EndPoint : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NextLevel();
    }

    /// <summary>
    /// Transition to the next level.
    /// </summary>
    private void NextLevel()
    {
        SceneController.Instance.UnloadCurrentScene();
        SceneController.Instance.LoadScene(nextScene);
    }
}
