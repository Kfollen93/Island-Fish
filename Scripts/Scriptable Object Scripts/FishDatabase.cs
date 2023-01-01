using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Database", menuName = "Scriptable Objects/Create Fish Database")]
public class FishDatabase : ScriptableObject
{
    public List<FishSO> FishSODatabase;
}
