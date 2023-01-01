using UnityEngine;

public static class FindInactiveObjects
{
    public static GameObject FindInActiveObjectByTag(string tag)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject inactiveGO in objects)
        {
            if (inactiveGO.CompareTag(tag))
            {
                return inactiveGO;
            }
        }
        return null;
    }
}
