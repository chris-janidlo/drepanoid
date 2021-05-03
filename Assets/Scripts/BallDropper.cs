using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

public class BallDropper : MonoBehaviour
{
    public float InitialDropDelay, RespawnDropDelay;
    public Transform SpawnPoint;

    public Ball BallPrefab;

    public GamePhaseVariable CurrentGamePhase;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(InitialDropDelay);
        CurrentGamePhase.Value = GamePhase.LevelPlaying;
        StartCoroutine(launchRoutine());
    }

    public void OnBallDied ()
    {
        StartCoroutine(launchRoutine());
    }

    IEnumerator launchRoutine ()
    {
        yield return new WaitForSeconds(RespawnDropDelay);
        if (CurrentGamePhase.Value == GamePhase.LevelPlaying) Instantiate(BallPrefab, SpawnPoint.position, Quaternion.identity);
    }
}
