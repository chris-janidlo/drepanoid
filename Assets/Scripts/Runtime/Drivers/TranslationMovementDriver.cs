using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

namespace Drepanoid.Drivers
{
    public class TranslationMovementDriver : MonoBehaviour
    {
        public float Velocity { get; private set; }
        public float PositionOnLine { get; private set; } = 0.5f;

        public float MaxVelocity, AccelerationTime;

        public float TimeUntilIdle;
        public float BallDeathPositionResetDelay;

        public AudioClip StartedIdlingSound, HitEndOfTrackSound;
        public AudioSource MovementSoundSource;

        public FloatVariable MovementAxis;
        public SoundEffectPlayer SoundEffectPlayer;

        float idleTimer;
        bool playedIdleSound;

        void FixedUpdate ()
        {
            int moveDirection = MathfExtra.TernarySign(MovementAxis.Value);

            bool movementStopped = moveDirection == 0;
            bool hitEndOfTrack = PositionOnLine == 0 && moveDirection == -1 || PositionOnLine == 1 && moveDirection == 1;

            if (movementStopped || hitEndOfTrack)
            {
                if (Velocity != 0 && hitEndOfTrack) SoundEffectPlayer.Play(HitEndOfTrackSound);

                Velocity = 0;
                MovementSoundSource.Stop();

                return;
            }

            if (Velocity == 0) MovementSoundSource.Play();

            idleTimer = 0;
            playedIdleSound = false;

            float accelerationThisFrame = MaxVelocity / AccelerationTime * MovementAxis.Value * Time.deltaTime;
            float unclampedVelocity = moveDirection != Mathf.Sign(Velocity)
                ? accelerationThisFrame
                : Velocity + accelerationThisFrame;

            Velocity = Mathf.Clamp(unclampedVelocity, -MaxVelocity, MaxVelocity);
            PositionOnLine = Mathf.Clamp(PositionOnLine + Velocity * Time.deltaTime, 0, 1);
        }

        void Update ()
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= TimeUntilIdle && !playedIdleSound)
            {
                SoundEffectPlayer.Play(StartedIdlingSound);
                playedIdleSound = true;
            }
        }

        public void OnBallDied ()
        {
            StartCoroutine(ballDeathRoutine());
        }

        IEnumerator ballDeathRoutine ()
        {
            yield return new WaitForSeconds(BallDeathPositionResetDelay);
            PositionOnLine = 0.5f;
        }
    }
}
