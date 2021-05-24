using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

public class BallDropper : MonoBehaviour
{
    public float RespawnDropDelay;
    public Transform SpawnPoint;

    public Ball BallPrefab;
    public SceneTransitionHelper SceneTransitionHelper;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(SceneTransitionHelper.LevelLoadAnimationTime);
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
