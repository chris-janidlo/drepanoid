using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public float DropDelay;
    public Transform SpawnPoint;

    public Ball BallPrefab;

    void Start ()
    {
        StartCoroutine(launchRoutine());
    }

    public void OnBallDied ()
    {
        StartCoroutine(launchRoutine());
    }

    IEnumerator launchRoutine ()
    {
        yield return new WaitForSeconds(DropDelay);
        Instantiate(BallPrefab, SpawnPoint.position, Quaternion.identity);
    }
}
