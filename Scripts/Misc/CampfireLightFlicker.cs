using UnityEngine;

public class CampfireLightFlicker : MonoBehaviour
{
    private Light lightSource;
    private Vector3 startPosition = new Vector3(0f, 0.90f, -0.05f);
    private Vector3 endPosition = new Vector3(0f, 1.45f, -0.05f);
    [SerializeField] private float pingPongSpeed = 1.45f;
    [SerializeField] private float flickerSpeed = 0.25f;
    [SerializeField] private float flickerMaxIntensity = 3f;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        InvokeRepeating("LightIntensityFlicker", 0, flickerSpeed);
    }

    private void Update()
    {
        PingPongLightPosition();
    }

    private void PingPongLightPosition()
    {
        transform.localPosition = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(Time.time, pingPongSpeed));
    }

    private void LightIntensityFlicker()
    {
        var rand = Random.Range(2.5f, flickerMaxIntensity);
        lightSource.intensity = rand;
    }
}
