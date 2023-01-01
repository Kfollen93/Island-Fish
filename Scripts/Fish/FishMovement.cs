using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public GameObject fish;
    public GameObject[] waypoints;
    public float speed = 1.0f;
    private int waypointsIndex;
    public Transform playerTransform;
    private Vector3 currentPoint;
    private float step;
    private bool swimmingAwayFromPlayer;
    private readonly float playerDetectionRadius = 4f;

    private void Update()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player").transform;

        if (!swimmingAwayFromPlayer)
        {
            FishSwim();
            DetectPlayer();
        }
        else
        {
            MoveFishAwayFromPlayer();
        }
    }

    private void FishSwim()
    {
        step = speed * Time.deltaTime;

        currentPoint = waypoints[waypointsIndex].transform.position;
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, currentPoint, step),
                  Quaternion.LookRotation(transform.position - currentPoint));
        float dist = Vector3.Distance(waypoints[waypointsIndex].transform.position, transform.position);

        if (dist <= 0.5f)
        {
            waypointsIndex++;
        }

        if (waypointsIndex >= waypoints.Length)
        {
            waypointsIndex = 0;
        }
    }

    private void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                swimmingAwayFromPlayer = true;
            }
        }
    }

    private void MoveFishAwayFromPlayer()
    {
        speed = 3f;
        float furthestPoint = 0;
        int furthestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector3.Distance(waypoints[i].transform.position, playerTransform.position);

            if (dist > furthestPoint)
            {
                furthestPoint = dist;
                furthestIndex = i;
            }
        }

        Vector3 furthestPlaceToMove = waypoints[furthestIndex].transform.position;
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, furthestPlaceToMove, step), Quaternion.LookRotation(transform.position - furthestPlaceToMove));
        Vector3 fishNewLocationPos = waypoints[furthestIndex].transform.position - transform.position;
        float fishDistanceToWaypoint = fishNewLocationPos.sqrMagnitude;
        if (fishDistanceToWaypoint <= 0.5f)
        {
            swimmingAwayFromPlayer = false;
            speed = 1f;
        }
    }
}
