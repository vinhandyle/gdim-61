using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ability-granting statues at the end of each level.
/// </summary>
public class EndPoint : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private int lvl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GrantAbility();
        NextLevel();
    }

    // Give the player an ability.
    private void GrantAbility()
    {
        // TODO: Add abilities
        switch (lvl)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;
        }
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
