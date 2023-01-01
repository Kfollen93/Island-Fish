using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastLineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _coastLineWaves;
    [SerializeField] private Transform[] coastLineTransforms;
    [SerializeField] private Transform player;
    [SerializeField] private SoundDataSO _soundData;
    private Transform closestAudio = null;
    private float distanceFromClosestCoastAudioToPlayer = 0;
    public float minDistance = 3f;

    private void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        closestAudio = GetClosestAudioHolder(coastLineTransforms);
        distanceFromClosestCoastAudioToPlayer = Vector3.Distance(closestAudio.position, player.position);
        _coastLineWaves.volume = (1f - Mathf.Clamp01(distanceFromClosestCoastAudioToPlayer / minDistance)) * _soundData.soundVolume;
    }

    private Transform GetClosestAudioHolder(Transform[] coastLineTransforms)
    {
        Transform closestCoastAudio = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3 currentPosition = player.position;

        for (int i = 0; i < coastLineTransforms.Length; i++)
        {
            Vector3 directionToTarget = coastLineTransforms[i].position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestCoastAudio = coastLineTransforms[i];
            }
        }

        return closestCoastAudio;
    }
}
