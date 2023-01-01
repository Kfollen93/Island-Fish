using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set of Fish", menuName = "Scriptable Objects/Create Fish Set")]
public class FishSetSO : ScriptableObject
{
    public List<FishSO> listOfFish;
    public FishSO GetRandomFishFromSet() 
    {
        return listOfFish[Random.Range(0, listOfFish.Count)];
    }

    //private void OnValidate()
    //{
    //    foreach (FishSO fish in listOfFish)
    //    {
    //        Debug.Log("Type " + fish.fishName);
    //        Debug.Log($"{fish.spawnChance:0.00} % chance to spawn");
    //        Debug.Log($"{Random.Range(fish.minWeight, fish.maxWeight):0.00} lbs");
    //    }
    //}
}