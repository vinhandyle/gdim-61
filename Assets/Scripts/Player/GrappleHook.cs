using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] private float grappleDetectDistance;
    [SerializeField] private float launchVelocity;
    private bool inGame = false;

    private Rigidbody2D rb;
    private BoxCollider2D cc;
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        inGame = true;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<BoxCollider2D>();
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transform closestPoint = GetNearestTarget(grappleDetectDistance);
            if (closestPoint != null)
            {
                Debug.DrawLine(closestPoint.position, transform.position, Color.red, 2f, false);
                // TODO: Animations for the hook itself
                Vector2 midpoint = Vector2.Lerp(transform.position, closestPoint.position, 0.5f);
                Vector2 direction = closestPoint.position - transform.position;
                direction.Normalize();
                StartCoroutine(MoveOverTime(midpoint, 0.3f));
            }
        }
    }

    /// <summary>
    /// Move the player to a specific end point in a fixed amount of time.
    /// </summary>
    // TODO: this code will make the player go through walls
    IEnumerator MoveOverTime(Vector2 endPos, float duration)
    {
        float currentTime = 0;
        bool prematureEnd = false;
        Vector2 startPos = rb.position;
        Vector2 direction = endPos - startPos;
        while (currentTime < duration)
        {
            // See if we are going to hit a wall on the way there
            RaycastHit2D detect = Physics2D.Raycast(startPos, direction, direction.magnitude, LayerMask.GetMask("Ground"));
            if (detect.collider != null)
            {
                prematureEnd = true;
                break;
            }

            rb.position = Vector2.Lerp(startPos, endPos, (currentTime / duration));
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(!prematureEnd) pc.AddForce(direction * launchVelocity, 0.5f);
    }

    /// <summary>
    /// Returns the closest grapple point within the given distance.
    /// </summary>
    private Transform GetNearestTarget(float distance)
    {
        if (transform == null) return null;

        Transform bestTarget = null;
        float closestDistanceSqr = distance;
        Vector3 currentPosition = transform.position;

        GameObject[] grapplePoints = GameObject.FindGameObjectsWithTag("GrapplePoint"); // Get all GameObjects that are tagged as "GrapplePoint"
        GameObject[] grapplePoints2 = GameObject.FindGameObjectsWithTag("Enemy"); // Get all GameObjects that are tagged as "GrapplePoint"
        grapplePoints = grapplePoints.Concat(grapplePoints2).ToArray();
        foreach (GameObject grapplePoint in grapplePoints) // For each grapplePoint in the array of point
        {
            Vector2 directionToTarget = grapplePoint.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = grapplePoint.transform;
            }
        }

        return bestTarget;
    }
}
