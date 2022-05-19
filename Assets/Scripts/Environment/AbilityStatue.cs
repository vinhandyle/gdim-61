using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the objects that unlock player abilities.
/// </summary>
public class AbilityStatue : Checkpoint
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            switch (int.Parse(SceneController.Instance.currentScene.Split(' ')[1]))
            {
                case 1:
                    Debug.Log("Unlocked Flame Dash");
                    Controls.Instance.dashUnlocked = true;
                    break;

                case 2:
                    Debug.Log("Unlocked Shell Smash");
                    Controls.Instance.slamUnlocked = true;
                    break;

                case 3:
                    Debug.Log("Unlocked Lightning Hook");
                    Controls.Instance.grappleUnlocked = true;
                    break;

                case 4:
                    Debug.Log("Unlocked Tiger Balm");
                    Controls.Instance.spAttackUnlocked = true;
                    break;
            }
        }
    }
}
