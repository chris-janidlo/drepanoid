using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms.SceneMgmt;

namespace Drepanoid
{
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

        public SoundEffect SoundEffect;

        public Vector2Variable SceneChangeDirection, CameraTrackingPosition;
        public StringVariable SceneTransitionTargetTag;
        public float MinBallTransitionSpeed;

        public Vector2Event BallSpawned;
        public SceneTransitionHelper SceneTransitionHelper;
        public SoundEffectPlayer SoundEffectPlayer;
        public Collider2D Collider;

        float timeSinceLastSpawn;
        bool isSpawnPoint;

        IEnumerator Start ()
        {
            if (SceneTransitionTargetTag.Value == Tag)
            {
                isSpawnPoint = true;
                yield return new WaitForSeconds(SceneTransitionHelper.LevelLoadAnimationTime);
                StartCoroutine(respawnRoutine());
                CameraTrackingPosition.Value = transform.position;
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

            SoundEffectPlayer.Play(SoundEffect);

            Collider.enabled = false;
            float speedInDirectionOfAdjacentScene = Vector2.Dot(ball.Velocity, AdjacentSceneDirection); // from https://docs.unity3d.com/2019.3/Documentation/Manual/AmountVectorMagnitudeInAnotherDirection.html
            ball.Velocity = AdjacentSceneDirection * Mathf.Max(speedInDirectionOfAdjacentScene, MinBallTransitionSpeed);
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
            BallSpawned.Raise(transform.position);
            timeSinceLastSpawn = 0;
        }
    }
}