using Pinwheel.Poseidon;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class WaterAreaMesh : MonoBehaviour
{
    [InfoBox("This script should be placed on an empty GO that is a child of the GO that has the PWater Script on it.\n" +
    "Then add MeshCollider and drag/drop the PWater script from the water GO parent, and drag the mesh collider on this GO into the slot.\n" +
    "This will only work when using 'Area' type of water, if it's Tileable Plane then just use box collider triggers since it would be a flat square.")]

    public PWater pWater;
    public MeshCollider meshCollider;

    void OnEnable() => meshCollider.sharedMesh = pWater.Mesh;
}
