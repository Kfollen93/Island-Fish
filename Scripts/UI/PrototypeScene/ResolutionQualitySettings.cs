using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionQualitySettings : MonoBehaviour
{
    public Toggle fullScreenToggle, vsyncToggle;
    public List<ResolutionItem> resolutions = new List<ResolutionItem>();
    private int currentResolution;
    public TMP_Text resolutionLabel;

    private void Start()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
            vsyncToggle.isOn = false;
        else
            vsyncToggle.isOn = true;

        bool foundResolution = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontalResolution && Screen.height == resolutions[i].verticalResolution)
            {
                foundResolution = true;
                currentResolution = i;
                UpdateResolutionLabelText();
            }
        }

        if (!foundResolution)
        {
            ResolutionItem newResolution = new ResolutionItem()
            {
                horizontalResolution = Screen.width,
                verticalResolution = Screen.height,
            };
            resolutions.Add(newResolution);
            currentResolution = resolutions.Count - 1;
            UpdateResolutionLabelText();
        }
    }

    public void ResoultionLeftButton()
    {
        currentResolution--;
        if (currentResolution < 0) currentResolution = 0;
        UpdateResolutionLabelText();
    }
        

    public void ResoultionRightButton()
    {
        currentResolution++;
        if (currentResolution > resolutions.Count - 1) currentResolution = resolutions.Count - 1;
        UpdateResolutionLabelText();
    }
        

    public void UpdateResolutionLabelText() => 
        resolutionLabel.text = resolutions[currentResolution].horizontalResolution.ToString() + " x " + resolutions[currentResolution].verticalResolution.ToString();

    public void ApplyScreenSettings()
    {
        //Screen.fullScreen = fullScreenToggle.isOn;
        if (vsyncToggle.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        Screen.SetResolution(resolutions[currentResolution].horizontalResolution, resolutions[currentResolution].verticalResolution, fullScreenToggle.isOn);
    }
}

[Serializable]
public class ResolutionItem
{
    public int horizontalResolution, verticalResolution;
}
