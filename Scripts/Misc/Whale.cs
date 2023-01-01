using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Whale : MonoBehaviour
{
    private float jumpPower = 15f;
    private int numOfJumps = 1;
    private float jumpDuration = 3.5f;
    private Vector3 inAirRotation = new Vector3(75f, 100f, 0f);
    private float rotDuration = 3f;
    private Vector3[] startingPositions = new Vector3[3]
    { 
        new Vector3(64.06f, -6.41f, 138.81f),
        new Vector3(-5.47f, -6.41f, 159.93f),
        new Vector3(152.2f, -6.41f, 36.4f)
    };
    private Vector3[] finalPositions = new Vector3[3]
    {
        new Vector3(80f, -5f, 132f),
        new Vector3(10.2f, -5f, 153.12f),
        new Vector3(168.14f, -5f, 29.59f)
    };
    private Quaternion startingRotation = Quaternion.Euler(-90f, 0f, 90f);
    // Dictionary is to map so that Index 0 of startingPositions will always go to Index 0 of endPositions, and so on.
    private Dictionary<int, Vector3> mapping = new Dictionary<int, Vector3>();
    private WaitForSeconds timeToWaitBeforeWhaleJump = new WaitForSeconds(60f);

    void Start()
    {
        for (int i = 0; i < 3; i++)
            mapping.Add(i, finalPositions[i]);

        StartCoroutine(DoWhaleJump());
    }

    private IEnumerator DoWhaleJump()
    {
        // Do jump once without needing to wait for first time game starts.
        transform.DOLocalJump(mapping[0], jumpPower, numOfJumps, jumpDuration);
        transform.DORotate(inAirRotation, rotDuration);

        while (true)
        {
            yield return timeToWaitBeforeWhaleJump;

            int index = Random.Range(0, 3);
            transform.SetPositionAndRotation(startingPositions[index], startingRotation);

            transform.DOLocalJump(mapping[index], jumpPower, numOfJumps, jumpDuration);
            transform.DORotate(inAirRotation, rotDuration);
        }
    }
}
