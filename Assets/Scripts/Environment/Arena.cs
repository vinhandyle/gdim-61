using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An area that requires the player to defeat all enemies to proceed.
/// </summary>
public class Arena : MonoBehaviour
{
    [SerializeField] private List<GameObject> gates;
    [SerializeField] private List<ArenaWave> waves;
    [SerializeField] private float timeBetweenWaves;

    [SerializeField] private int currentWave;
    [SerializeField] private bool breather; // Safe period between waves
    [SerializeField] private bool completed;

    private void Awake()
    {
        // Need to do this so that arena enemies can set their respawn positions correctly
        waves.ForEach(wave => wave.container.SetActive(true));
        waves.ForEach(wave => wave.container.SetActive(false));
    }

    private void Update()
    {
        if (!GetComponent<Collider2D>().enabled && !completed)
        {
            if (currentWave == waves.Count)
            {
                completed = true;
                gates.ForEach(gate => gate.SetActive(false));
            }
            else if (!breather && waves[currentWave].enemies.TrueForAll(enemy => !enemy.gameObject.activeSelf) && ++currentWave < waves.Count)
            {                
                StartCoroutine(LoadNextWave());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waves[0].container.SetActive(true);
            gates.ForEach(gate => gate.SetActive(true));
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Resets progression of the arena if not completed yet.
    /// </summary>
    public void ResetArena()
    {
        if (!completed)
        {
            waves.ForEach(wave => wave.container.SetActive(false));
            gates.ForEach(gate => gate.SetActive(false));

            GetComponent<BoxCollider2D>().enabled = true;
            currentWave = 0;
        }
    }

    /// <summary>
    /// Loads the next wave after a short period of time for the player to recompose.
    /// </summary>
    private IEnumerator LoadNextWave()
    {
        breather = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        waves[currentWave].container.SetActive(true);
        breather = false;
    }
}

/// <summary>
/// Defines a wave of enemies.
/// </summary>
[System.Serializable]
public class ArenaWave
{
    public GameObject container;
    public List<Enemy> enemies;
}