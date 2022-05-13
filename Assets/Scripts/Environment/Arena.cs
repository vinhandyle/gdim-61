using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An area that requires the player to defeat all enemies to proceed.
/// </summary>
public class Arena : MonoBehaviour
{
    [SerializeField] private List<GameObject> gates;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private bool completed;

    // For camera stuff   might need to optimize later
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraLockPoint;
    private CameraLVL1 cameraScript;

    private void Start()
    {
        cameraScript = mainCamera.GetComponent<CameraLVL1>();
    }

    private void Update()
    {
        if (!completed)
        {
            cameraScript.RestorePlayerTarget();
            if (enemies.TrueForAll(enemy => enemy == null))
            {
                completed = true;
                gates.ForEach(gate => gate.SetActive(false));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemies.ForEach(enemy => enemy.gameObject.SetActive(true));
            gates.ForEach(gate => gate.SetActive(true));
            GetComponent<BoxCollider2D>().enabled = false;
            cameraScript.SetTarget(cameraLockPoint);
        }
    }
}
