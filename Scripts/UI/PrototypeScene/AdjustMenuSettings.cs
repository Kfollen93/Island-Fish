using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Sirenix.OdinInspector;

public class AdjustMenuSettings : MonoBehaviour
{
    [Title("Cinemachine")]
    [SerializeField] CinemachineFreeLook cmCam;
    [SerializeField] private Slider cmCamSliderX;
    [SerializeField] private Slider cmCamSliderY;

    void Update()
    {
        cmCam.m_XAxis.m_MaxSpeed = cmCamSliderX.value;
        cmCam.m_YAxis.m_MaxSpeed = cmCamSliderY.value;
    }
}
