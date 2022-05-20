using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines parallax behavior for backgrounds.
/// </summary>
public class Background : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float scale;
    private float pivotPos;

    private void Awake()
    {
        pivotPos = transform.position.x;
    }

    void FixedUpdate()
    {
        // Scrolling is determined by scaling distance from the pivot
        // and adjusting background relative to the anchor
        float delta = (anchor.position.x - pivotPos) * scale;
        transform.position = new Vector3(anchor.position.x - delta, transform.position.y, transform.position.z);
    }
}
