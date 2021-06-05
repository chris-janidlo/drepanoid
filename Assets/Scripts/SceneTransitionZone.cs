using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms.SceneMgmt;

public class SceneTransitionZone : MonoBehaviour
{
    public Vector2 SpawnDirection => -AdjacentSceneDirection;

    public SceneField AdjacentScene;
    [Tooltip("This has 2 uses: 1) when the ball hits this (ie, this is treated as an exit), this is the direction the camera should move when transitioning to the next scene, and 2) if the value of SceneChangeDirection is the exact negative of this, then this will be treated as the spawn point for the duration that this scene is loaded")]
    public Vector2 AdjacentSceneDirection;

    public float BallSpawnDelay, ExitDisabledTimeAfterSpawning, BallSpawnInitialVelocity;
    public Ball BallPrefab;

    public Vector2Variable SceneChangeDirection, CameraTrackingPosition;
    public SceneTransitionHelper SceneTransitionHelper;
    public Collider2D Collider;

    float timeSinceLastSpawn;
    bool isSpawnPoint;

    IEnumerator Start ()
    {
        if (SceneChangeDirection.Value == SpawnDirection)
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
        ball.Velocity = ball.Velocity.magnitude * AdjacentSceneDirection;
        ball.DespawnVictoriously();

        SceneChangeDirection.Value = AdjacentSceneDirection;
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
