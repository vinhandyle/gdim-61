using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField] private GameObject debris;
    [SerializeField] private GameObject rDebris;
    [SerializeField] private float spawnSpeed;
    [SerializeField] private bool resetMode;
    

    void Start()
    {
        if (!resetMode)
        {
            StartCoroutine(StartDebris());
        }

        else
        {
            RepeatingDebris();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnDebris()
    {
        GameObject deb = Instantiate(debris) as GameObject;
        deb.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    IEnumerator StartDebris()
    {

        while (true)
        {
            yield return new WaitForSeconds(spawnSpeed);
            SpawnDebris();
        }

    }


    private void RepeatingDebris()
    {
        GameObject rdeb = Instantiate(rDebris) as GameObject;
        rdeb.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
