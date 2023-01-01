using Pinwheel.Poseidon;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    private Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    private PWater water;
    private Vector3 worldPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Water"))
            water = col.GetComponentInParent<PWater>();
        else 
            water = null;
    }

    private void FixedUpdate()
    {
        float waveHeight = worldPos.y;
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        GetWaveHeight();

        bool isBobberTouchingWater = transform.position.y <= worldPos.y;
        if (isBobberTouchingWater)
        {
            rb.isKinematic = true;
            StartCoroutine(TurnOffKinematic());
        }
    }

    private void GetWaveHeight()
    {
        if (water == null) return;

        Vector3 localPos = water.transform.InverseTransformPoint(transform.position);
        localPos.y = 0;
        localPos = water.GetLocalVertexPosition(localPos, true);
        worldPos = water.transform.TransformPoint(localPos);
    }

    private IEnumerator TurnOffKinematic()
    {
        yield return new WaitForSeconds(.5f);
        rb.isKinematic = false;
    }
}
