using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms.SceneMgmt;

public class SceneTransitionZone : MonoBehaviour
{
    public Vector2 SpawnDirection => -AdjacentSceneDirection;

    [Tooltip("When moving to the next level, the SceneTransitionTargetTag is set to TargetTransitionZoneTag. When arriving in a level, the transition zone with the Tag value that matches SceneTransitionTargetTag will be used as the spawn transition zone.")]
    public string Tag;

    public SceneField AdjacentScene;
    [Tooltip("When moving to the next level, the SceneTransitionTargetTag is set to TargetTransitionZoneTag. When arriving in a level, the transition zone with the Tag value that matches SceneTransitionTargetTag will be used as the spawn transition zone.")]
    public string TargetTransitionZoneTag;

    [Tooltip("The direction that the camera should travel when moving to the adjacent level, and the opposite of the direction the camera should travel when arriving here")]
    public Vector2 AdjacentSceneDirection;

    public float BallSpawnDelay, ExitDisabledTimeAfterSpawning, BallSpawnInitialVelocity;
    public Ball BallPrefab;

    public Vector2Variable SceneChangeDirection, CameraTrackingPosition;
    public StringVariable SceneTransitionTargetTag;
    public SceneTransitionHelper SceneTransitionHelper;
    public Collider2D Collider;

    float timeSinceLastSpawn;
    bool isSpawnPoint;

    IEnumerator Start ()
    {
        if (SceneTransitionTargetTag.Value == Tag)
        {
            isSpawnPoint = true;
            yield return new WaitForSeconds(SceneTransitionHelper.LevelLoadAnimationTime);
            CameraTrackingPosition.Value = transform.position;
            yield return respawnRoutine();
        }
    }

    void Update ()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (timeSinceLastSpawn <= ExitDisabledTimeAfterSpawning) return;

        var ball = collision.GetComponent<Ball>();
        if (ball == null) return;

        Collider.enabled = false;
        ball.Velocity = Vector3.Project(ball.Velocity, AdjacentSceneDirection);
        ball.DespawnVictoriously();

        SceneChangeDirection.Value = AdjacentSceneDirection;
        SceneTransitionTargetTag.Value = TargetTransitionZoneTag;
        StartCoroutine(SceneTransitionHelper.UnloadCurrentLevelAndLoadNextRoutine(AdjacentScene));
    }

    public void OnBallDied ()
    {
        if (isSpawnPoint) StartCoroutine(respawnRoutine());
    }

    IEnumerator respawnRoutine ()
    {
        yield return new WaitForSeconds(BallSpawnDelay);
        var ball = Instantiate(BallPrefab, transform.position, Quaternion.identity);
        ball.Velocity = SpawnDirection * BallSpawnInitialVelocity;
        timeSinceLastSpawn = 0;
    }
}
