using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField] private GameObject debris;
    [SerializeField] private GameObject rDebris;
    [SerializeField] private float initialBuffer;
    [SerializeField] private float spawnSpeed;
    [SerializeField] private float damage;
    [Tooltip("0: One-time, 1: Repeats once broken, 2: Repeats once offscreen")]
    [SerializeField] private int type;
    [Tooltip("0: Inactive, 1: Activated, 2: Active")]
    [SerializeField] private int mode;

    private void Update()
    {
        if (mode == 1)
        {
            mode = 2;

            switch (type)
            {
                case 0:
                    SpawnDebris();
                    break;

                case 1:
                    StartCoroutine(StartDebris());
                    break;

                case 2:
                    RepeatingDebris();
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (mode == 0) mode = 1;
        }
    }

    public void ResetSpawner()
    {
        mode = 0;
    }

    private void SpawnDebris()
    {
        GameObject deb = Instantiate(debris) as GameObject;
        deb.GetComponent<Debris>().SetDamage(damage);
        deb.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    IEnumerator StartDebris()
    {
        yield return new WaitForSeconds(initialBuffer);

        while (mode != 0)
        {
            SpawnDebris();
            yield return new WaitForSeconds(spawnSpeed);
        }
    }


    private void RepeatingDebris()
    {
        GameObject rdeb = Instantiate(rDebris) as GameObject;
        rdeb.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
