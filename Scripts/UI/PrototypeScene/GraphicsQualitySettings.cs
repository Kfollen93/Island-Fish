using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class GraphicsQualitySettings : MonoBehaviour
{
    public RenderPipelineAsset[] qualitySettings;
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void ChangeQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualitySettings[value];
    }
}
