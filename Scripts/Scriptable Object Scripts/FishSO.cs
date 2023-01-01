using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Fish", menuName = "Scriptable Objects/Create Fish")]
public class FishSO : ScriptableObject
{
    [BoxGroup("Basic Info")]
    [LabelWidth(100)]
    public string fishName;

    [BoxGroup("Basic Info")]
    [LabelWidth(100)]
    [TextArea]
    public string description;

    [HorizontalGroup("Fish Data", 75)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite fishImage;

    [VerticalGroup("Fish Data/Stats")]
    [LabelWidth(100)]
    [GUIColor(0.5f, 1f, 0.5f)]
    public float spawnChance;

    [VerticalGroup("Fish Data/Stats")]
    [LabelWidth(100)]
    [Range(1, 10f)]
    [GUIColor(0.5f, 1f, 0.5f)]
    public float reelInDifficulty;

    [InfoBox("MinLength and MaxLength use Integer represented in Inches.")]
    [VerticalGroup("Fish Data/Stats")]
    [LabelWidth(100)]
    [Range(1, 60)]
    [GUIColor(1f, 1f, 0.5f)]
    public int minLength;
    [VerticalGroup("Fish Data/Stats")]
    [LabelWidth(100)]
    [Range(1, 60)]
    [GUIColor(1f, 1f, 0.5f)]
    public int maxLength;
}