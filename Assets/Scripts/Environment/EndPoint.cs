using UnityEngine;

/// <summary>
/// Defines an object that sends the player to the next level.
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
