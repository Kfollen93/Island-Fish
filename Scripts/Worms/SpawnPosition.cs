using UnityEngine;
// This should be attached to an empty gameobject that has box collider on it for spawning worm areas.
public class SpawnPosition : MonoBehaviour
{
    private Collider m_Collider;
    public LayerMask groundMask;

    private void OnEnable()
    {
        m_Collider = GetComponent<Collider>();
    }

    public Bounds GetColliderBounds() => m_Collider.bounds;

}