using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("NOTE: Cycle Length must be less than or equal to Time To Wait Before Respawn.")]
    [SerializeField] private float cycleLength = 5f;
    [SerializeField] private float timeToWaitBeforeRespawn = 5f;
    [SerializeField] private Vector3 startingPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 finalPosition = new Vector3(0f, 0f, 0f);
    private BirdFlyAnims birdFlyingAnimsScript;

    private void Awake() => birdFlyingAnimsScript = GetComponent<BirdFlyAnims>();

    void Start() => StartCoroutine(FlyBird());

    private IEnumerator FlyBird()
    {
        // Initial start outside of loop.
        birdFlyingAnimsScript.PlayBirdFlyingAnims();
        transform.DOMove(finalPosition, cycleLength);

        while (true)
        {
            yield return new WaitForSeconds(timeToWaitBeforeRespawn);
            transform.position = startingPosition;
            birdFlyingAnimsScript.PlayBirdFlyingAnims();
            transform.DOMove(finalPosition, cycleLength);
        }
    }
}