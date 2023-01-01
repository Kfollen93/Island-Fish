using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable All

public class ItemPickup : MonoBehaviour
{
    private CharacterAnimations characterAnimationsScript;
    public GameObject[] worms;
    private List<GameObject> isActive, nonActive;
    private int wormArraySize;
    private GameObject targetWorm;
    private float timer = 5f;
    [HideInInspector] public static bool firstTimePickingUpWorm; // Used to send display message.
    private bool hasAWormBeenAdded;
    private SpawnPosition[] spawnPosScripts;
    private readonly WaitForSeconds lengthOfPickUpAnimation = new WaitForSeconds(1f);
    private PlayerInput playerInput;
    private WormCountUI wormScript;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        StartCoroutine(FindWormManager());
        characterAnimationsScript = FindObjectOfType<CharacterAnimations>();

        isActive = new List<GameObject>(worms.Length);
        nonActive = new List<GameObject>(worms.Length);

        spawnPosScripts = FindObjectsOfType<SpawnPosition>();

        for (int i = 0; i < spawnPosScripts.Length; i++)
        {
            foreach (GameObject worm in worms)
            {
                var clone = Instantiate(worm);
                clone.GetComponent<MoveWorm>().Init(spawnPosScripts[i]);
                isActive.Add(clone);
            }
        }
        
        wormArraySize = worms.Length;
    }

    private void Update()
    {
        if (InputManager.Instance.InteractCurrentlyPressed && targetWorm)
        {
            characterAnimationsScript.PlayPickUpAnim();
            GameSounds.Instance.PlayGeneralBoopSFX();
            StartCoroutine(ReEnablePlayerControls());
            targetWorm.SetActive(false);
            isActive.Remove(targetWorm);
            nonActive.Add(targetWorm);
            firstTimePickingUpWorm = true;
            hasAWormBeenAdded = true;
            targetWorm = null;
        }

        if (InputManager.Instance.InteractHasBeenLetGo && hasAWormBeenAdded)
        {
            wormScript.AddWorm(1);
            hasAWormBeenAdded = false;
        }

        SetWormsActiveTimer();
    }

    private IEnumerator FindWormManager()
    {
        // Wait a frame after loading PlayerUI scene additively, otherwise will be NRE.
        yield return null;
        wormScript = GameObject.FindWithTag("Worm Manager").GetComponent<WormCountUI>();
    }

    private IEnumerator ReEnablePlayerControls()
    {
        playerInput.currentActionMap.Disable();
        yield return lengthOfPickUpAnimation;
        playerInput.currentActionMap.Enable();
    }

    private void SetWormsActiveTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            worms = GameObject.FindGameObjectsWithTag("Worm");
            int amountOfWormsInWorld = worms.Length;

            if (amountOfWormsInWorld < wormArraySize)
            {
                foreach (GameObject worm in nonActive)
                {
                    isActive.Add(worm);
                    worm.SetActive(true);
                }
                nonActive.Clear();
            }
            timer = 5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Worm"))
        {
            targetWorm = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetWorm = null;
    }
}
