using UnityEngine;
using Sirenix.OdinInspector;

public class RockTheBoat : MonoBehaviour
{
    [Title("Control Boat Rock Amount")]
    private Vector3 fromX = new Vector3(-94f, 0f, 0f);
    private Vector3 toX = new Vector3(-87f, 0f, 0f);
    [SerializeField] private float frequency = 1.0f;
    private Quaternion from, to;
    private float lerp;

    private void Start()
    {
        from = Quaternion.Euler(fromX);
        to = Quaternion.Euler(toX);  
    }

    private void Update()
    {
        lerp = 0.5f * (1.0f + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * frequency));
        transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }
}
