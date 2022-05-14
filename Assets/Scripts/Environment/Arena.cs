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

    private void Update()
    {
        if (!GetComponent<Collider2D>().enabled && !completed)
        {
            if (currentWave == waves.Count)
            {
                completed = true;
                gates.ForEach(gate => gate.SetActive(false));
            }
            else if (!breather && waves[currentWave].enemies.TrueForAll(enemy => enemy == null) && ++currentWave < waves.Count)
            {                
                StartCoroutine(LoadNextWave());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waves[0].enemies.ForEach(enemy => enemy.gameObject.SetActive(true));
            gates.ForEach(gate => gate.SetActive(true));
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private IEnumerator LoadNextWave()
    {
        breather = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        waves[currentWave].enemies.ForEach(enemy => enemy.gameObject.SetActive(true));
        breather = false;
    }
}

/// <summary>
/// Defines a wave of enemies.
/// </summary>
[System.Serializable]
public class ArenaWave
{
    public List<Enemy> enemies;
}