using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Transform anchor;

    void FixedUpdate()
    {
        transform.position = new Vector3(anchor.position.x, transform.position.y, transform.position.z);
    }
}
