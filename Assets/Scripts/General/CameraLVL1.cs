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
    private float startY;
    private float oldXOffset;
    private float oldYOffset;

    private void Start()
    {
        player = target;
        startY = transform.position.y + yOffset;
        transform.position = new Vector3(transform.position.x + 5.87f, startY, transform.position.z);
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
            xTarget = 5.87f;
        }
        float yTarget = target.position.y + yOffset;

        float xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * speed);
        // float yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * speed);
        transform.position = new Vector3(xNew, startY, transform.position.z);


        // The ugly way to change resolution
        // TODO: fix yOffset for each camera change
        // TODO: don't change orthopgraphic size, change height (they say this is more complicated must investigate further)
        if (target.transform.position.x > 222.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 212.9f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 13, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 162f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 10, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 104.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 12, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 90.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 4, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 90.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 4, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 90.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 4, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 90.4f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 4, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 73.5f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 10, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 61.6f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5, Time.deltaTime * 2);
        }
        else if (target.transform.position.x > 22.51f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 10, Time.deltaTime * 2);
        }
        else
        {
            startY = transform.position.y + yOffset;
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5, Time.deltaTime * 2);
        }
    }

    /// <summary>
    /// Sets the target for the camera to lock onto
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        oldYOffset = yOffset;
        target = newTarget;
        startY = newTarget.transform.position.y;
        yOffset = 0;
    }

    /// <summary>
    /// Sets the camera's target back to the player
    /// </summary>
    public void RestorePlayerTarget()
    {
        yOffset = oldYOffset;
        startY = transform.position.y + yOffset;
        target = player;
    }
}
