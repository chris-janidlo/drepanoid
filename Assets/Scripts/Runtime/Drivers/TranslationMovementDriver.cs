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

        public float MoveSoundPitchVariance;
        public Vector2 MoveSoundPitchRange;
        public float MoveSoundVolume;
        public TransitionableFloat MoveSoundStartUpTransition;

        public BagRandomizer<SoundEffect> StartedMovingSounds;
        public SoundEffect StartedIdlingSound, HitEndOfTrackSound;
        public AudioSource MovementSoundSource;

        public FloatVariable MovementAxis;
        public BoolVariable DeathGlitchEffectIsOn;
        public SoundEffectPlayer SoundEffectPlayer;

        float idleTimer;
        bool playedIdleSound;

        float currentMoveSoundPitch;

        void Start ()
        {
            currentMoveSoundPitch = RandomExtra.Range(MoveSoundPitchRange);

            MoveSoundStartUpTransition.AttachMonoBehaviour(this);
        }

        void FixedUpdate ()
        {
            int moveDirection = MathfExtra.TernarySign(MovementAxis.Value);

            bool movementStopped = moveDirection == 0;
            bool hitEndOfTrack = PositionOnLine == 0 && moveDirection == -1 || PositionOnLine == 1 && moveDirection == 1;

            if (movementStopped || hitEndOfTrack)
            {
                if (Velocity != 0 && hitEndOfTrack) SoundEffectPlayer.Play(HitEndOfTrackSound);

                Velocity = 0;
                MovementSoundSource.volume = 0;

                return;
            }

            if (Velocity == 0) startSoundUp();

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
            if (!DeathGlitchEffectIsOn.Value) idleTimer += Time.deltaTime;

            if (idleTimer >= TimeUntilIdle && !playedIdleSound)
            {
                SoundEffectPlayer.Play(StartedIdlingSound);
                playedIdleSound = true;
            }
        }

        public void OnDeathReset ()
        {
            PositionOnLine = 0.5f;
        }

        void startSoundUp ()
        {
            IEnumerator routine ()
            {
                SoundEffectPlayer.Play(StartedMovingSounds.GetNext());
                MovementSoundSource.volume = MoveSoundVolume;
                MovementSoundSource.pitch = 0;

                currentMoveSoundPitch = Mathf.Clamp
                (
                    currentMoveSoundPitch + Random.Range(-MoveSoundPitchVariance, MoveSoundPitchVariance),
                    MoveSoundPitchRange.x,
                    MoveSoundPitchRange.y
                );

                MoveSoundStartUpTransition.FlashFromTo(0, currentMoveSoundPitch);
                
                while (MoveSoundStartUpTransition.Transitioning)
                {
                    MovementSoundSource.pitch = MoveSoundStartUpTransition.Value;
                    yield return null;
                }

                MovementSoundSource.pitch = currentMoveSoundPitch;
            }

            StopAllCoroutines();
            StartCoroutine(routine());
        }
    }
}
