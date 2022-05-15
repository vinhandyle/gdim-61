using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLVL1 : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] public Transform target;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float speed;

    private Transform player;
    [SerializeField] private Vector2 scale = Vector2.one;

    private void Start()
    {
        player = target;
        transform.position = new Vector3(transform.position.x + 5.87f, yOffset, transform.position.z);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 10, Time.deltaTime * 2);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 2, Time.deltaTime * 2);
        }

        float xTarget = target.position.x + xOffset;

        // Don't let the camera go past a specific point near the start of the level
        if (target.transform.position.x < 6)
        {
            xTarget = 6f;
        }
        else if(target.transform.position.x > 236)
        {
            xTarget = 236f;
        }

        float xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * speed);
        // float yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * speed);
        transform.position = new Vector3(xNew, yOffset, transform.position.z);
    }

    /// <summary>
    /// Sets the target for the camera to lock onto
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Sets the camera's target back to the player
    /// </summary>
    public void RestorePlayerTarget()
    {
        target = player;
    }
}
