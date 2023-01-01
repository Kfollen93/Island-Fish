using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenStoreTrigger : MonoBehaviour
{
    [SerializeField] private GameObject openStoreText;
    [SerializeField] private GameObject closeStoreText;
    private bool isStoreMenuOpen;
    public bool InStoreTrigger { get; private set; } // Used in DisplayItemInfo.cs
    [SerializeField] private CinemachineVirtualCamera cmVirtualShopCam;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private List<GameObject> shopItems;
    private int iterateShopItems = 0;
    private Vector3 defaultPosition = new Vector3(-9.75f, 4.916f, -3.976f);

    [SerializeField] private GameObject[] itemInfoButtons;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InStoreTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InStoreTrigger = false;
            openStoreText.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isStoreMenuOpen && InStoreTrigger && InputManager.Instance.InteractCurrentlyPressed)
        {
            if (playerInput == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                playerInput = player.GetComponent<PlayerInput>();
            }

            isStoreMenuOpen = true;
            // Switch to StoreControls
            playerInput.SwitchCurrentActionMap("ShopControls");
            // Enable Shop Camera
            cmVirtualShopCam.Priority = 2;
        }
        else if (!isStoreMenuOpen && InStoreTrigger)
        {
            closeStoreText.SetActive(false);
            openStoreText.SetActive(true);
        }
        else if (isStoreMenuOpen && InStoreTrigger && InputManager.Instance.exitShopPressed)
        {
            playerInput.SwitchCurrentActionMap("PlayerControls");
            cmVirtualShopCam.Priority = 1;
            isStoreMenuOpen = false;
            InputManager.Instance.exitShopPressed = false;
            cmVirtualShopCam.transform.position = defaultPosition;
            MouseStatus.HideCursor();
        }
        else if (isStoreMenuOpen)
        {
            openStoreText.SetActive(false);
            closeStoreText.SetActive(true);
            StrafeThroughShopItems();
            MouseStatus.DisplayCursor();
        }
    }

    private void StrafeThroughShopItems()
    {
        if (InputManager.Instance.scrollRight && iterateShopItems < shopItems.Count - 1)
        {
            iterateShopItems++;
            InputManager.Instance.scrollRight = false;
        }
        else if (InputManager.Instance.scrollRight && iterateShopItems == shopItems.Count - 1)
        {
            iterateShopItems = 0;
            InputManager.Instance.scrollRight = false;
        }
        else if (InputManager.Instance.scrollLeft && iterateShopItems == 0)
        {
            iterateShopItems = shopItems.Count - 1;
            InputManager.Instance.scrollLeft = false;
        }
        else if (InputManager.Instance.scrollLeft && iterateShopItems != 0)
        {
            iterateShopItems--;
            InputManager.Instance.scrollLeft = false;
        }

        float speedToMoveCam = 5f;
        Vector3 zCameraOffset = new Vector3(0f, 0f, 2.5f);
        cmVirtualShopCam.transform.position = Vector3.MoveTowards(cmVirtualShopCam.transform.position, shopItems[iterateShopItems].transform.position + zCameraOffset, speedToMoveCam * Time.deltaTime);
    }
}