using UnityEngine;

public class DisplayItemInfo : MonoBehaviour
{
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private GameObject[] shopItemsInfo;
    [SerializeField] private OpenStoreTrigger openStoreTriggerScript;

    private void Update()
    {
        if (openStoreTriggerScript.InStoreTrigger)
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                for (int i = 0; i < shopItems.Length; i++)
                {
                    if (hit.transform.name == shopItems[i].name)
                        shopItemsInfo[i].SetActive(true);
                    else
                        shopItemsInfo[i].SetActive(false);
                }
            }
            else
            {
                foreach (GameObject item in shopItemsInfo)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}