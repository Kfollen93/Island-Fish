using UnityEngine;

public class MoveWorm : MonoBehaviour
{
    private SpawnPosition wormBox;
    public LayerMask groundMask;
    private Vector3 boundsMin, boundsMax;
    private bool reachedDestination;
    private Vector3 moveTo;
    private bool initialPosSet;
    private readonly float moveSpeed = 0.35f;
    private float timer = 0f;
    private readonly float maxTimeBeforeSwitchingPos = 3f;

    public void Init(SpawnPosition box)
    {
        wormBox = box;
        transform.position = GetRandomNewPositionInsideBox();
    }

    private Vector3 GetRandomNewPositionInsideBox()
    {
        Bounds bounds = wormBox.GetColliderBounds();
        boundsMin = bounds.min;
        boundsMax = bounds.max;
        Vector3 randomPosWithinBoxBounds = new Vector3(Random.Range(boundsMin.x, boundsMax.x), Random.Range(boundsMin.y, boundsMax.y), Random.Range(boundsMin.z, boundsMax.z));

        Vector3 spawnPos = new Vector3();
        float depth = -0.25f; // A depth of 0 spawns worms directly at the level of the ground, -depth will spawn them a bit over, positive depth will spawn them below
        Ray ray = new Ray(randomPosWithinBoxBounds, Vector3.down);
        var isHit = Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask);

        if (isHit)
        {
            spawnPos = hit.point - hit.normal * depth;
        }

        return spawnPos;
    }

    private void Update()
    {
        if (!initialPosSet)
        {
            moveTo = GetRandomNewPositionInsideBox();
            initialPosSet = true;
        }

        if (!reachedDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * moveSpeed);

            timer += Time.deltaTime; // Added a timer here in case some worms get stuck from not being able to reach the destination
            if (Vector3.Distance(transform.position, moveTo) <= 0.5f || timer >= maxTimeBeforeSwitchingPos)
            {
                reachedDestination = true;
            }
        }
        else
        {
            moveTo = GetRandomNewPositionInsideBox();
            timer = 0f;
            reachedDestination = false;
        }
    }
}
