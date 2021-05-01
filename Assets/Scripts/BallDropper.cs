using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public float InitialDropDelay, RespawnDropDelay;
    public Transform SpawnPoint;

    public Ball BallPrefab;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(InitialDropDelay);
        StartCoroutine(launchRoutine());
    }

    public void OnBallDied ()
    {
        StartCoroutine(launchRoutine());
    }

    IEnumerator launchRoutine ()
    {
        yield return new WaitForSeconds(RespawnDropDelay);
        Instantiate(BallPrefab, SpawnPoint.position, Quaternion.identity);
    }
}
