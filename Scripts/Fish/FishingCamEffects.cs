using Cinemachine;
using UnityEngine;

public class FishingCamEffects : MonoBehaviour
{
    private CinemachineFreeLook freeLookCam;
    private CinemachineVirtualCamera topRig, midRig, bottomRig;
    private CinemachineBasicMultiChannelPerlin topNoise, midNoise, bottomNoise;
    [SerializeField] private BobberObject bobberScript;
    private bool isCamShaking;
    private readonly float minFOV = 72f;
    private readonly float defaultFOV = 75f;
    private readonly float speedOfBounce = 2f;

    private void Awake()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();

        topRig = freeLookCam.GetRig(0);
        topNoise = topRig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        midRig = freeLookCam.GetRig(1);
        midNoise = midRig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        bottomRig = freeLookCam.GetRig(2);
        bottomNoise = bottomRig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        ZoomToggle();
        CamShakeToggle();
    }

    private void CamShakeToggle()
    {
        if (bobberScript.isFishOnTheLine)
        {
            EnableCamShake(1f);
            freeLookCam.m_Lens.FieldOfView = 68f;
        }
        else if (isCamShaking && !bobberScript.isFishOnTheLine)
            DisableCamShake();
    }
    private void EnableCamShake(float amplitudeGain)
    {
        isCamShaking = true;
        topNoise.m_AmplitudeGain = amplitudeGain;
        midNoise.m_AmplitudeGain = amplitudeGain;
        bottomNoise.m_AmplitudeGain = amplitudeGain;
    }

    private void DisableCamShake()
    {
        isCamShaking = false;
        topNoise.m_AmplitudeGain = 0.0f;
        midNoise.m_AmplitudeGain = 0.0f;
        bottomNoise.m_AmplitudeGain = 0.0f;
    }

    private void ZoomToggle()
    {
        if (bobberScript.isBobberInWater)
            ZoomBounce();
        else if (!bobberScript.isBobberInWater && freeLookCam.m_Lens.FieldOfView != 75f)
            freeLookCam.m_Lens.FieldOfView = defaultFOV;
    }
    private void ZoomBounce() => freeLookCam.m_Lens.FieldOfView = Mathf.Lerp(minFOV, defaultFOV, Mathf.PingPong(Time.time / speedOfBounce, 1));
}
