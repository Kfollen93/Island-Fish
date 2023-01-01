using UnityEngine;
using Cinemachine;

public class SetPlayerFollow : MonoBehaviour
{
    private GameObject player;
    private GameObject lookTarget;
    private CinemachineFreeLook freeLookCam;

    void Start()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            lookTarget = GameObject.FindWithTag("LookTarget");

            if (player != null)
            {
                freeLookCam.LookAt = lookTarget.transform;
                freeLookCam.Follow = player.transform;
            }
        }
    }
}
