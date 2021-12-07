using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;
using crass;

namespace Drepanoid
{
    public class ScanlineVisuals : MonoBehaviour
    {
        public float MinDistanceToTurnOnParticles, HorizontalLineVerticalOffset, SceneLoadDelayBeforeLoadingLines;

        public TransitionableFloat LineTransition;

        public Transform Levitator, LeftAnchor, RightAnchor, LevitatorLineConnector;
        public ParticleSystem LevitatorParticles;
        public LineRenderer HorizontalLine, VerticalLine, MagnetRangeLine;
        public SpriteRenderer LeftAnchorSprite, RightAnchorSprite;

        public Vector2Variable CameraTrackingPosition;
        public ScanlinePhysics Physics;

        float effectiveMagnetismRange => Physics.BallMagnetismRange * Physics.BallMagnetismAmount;

        bool active = true, animatingLine = true;

        IEnumerator Start ()
        {
            setLevitatorState(false);

            LeftAnchorSprite.transform.position = LeftAnchor.position + Vector3.left * effectiveMagnetismRange;
            RightAnchorSprite.transform.position = RightAnchor.position + Vector3.right * effectiveMagnetismRange;

            yield return new WaitForSeconds(SceneLoadDelayBeforeLoadingLines);
            yield return lineAnimation(true);
        }

        void FixedUpdate ()
        {
            if (!active) return;

            bool particlesOn = Vector2.Distance(transform.position, CameraTrackingPosition.Value) < MinDistanceToTurnOnParticles;

            setLevitatorState(particlesOn);
            Levitator.position = transform.position;

            if (animatingLine) return;

            VerticalLine.SetPosition(0, LevitatorLineConnector.position);
            VerticalLine.SetPosition(1, getLineMeetupPoint());
            MagnetRangeLine.SetPosition(0, magnetRangeMidpoint() + Vector3.left * effectiveMagnetismRange);
            MagnetRangeLine.SetPosition(1, magnetRangeMidpoint() + Vector3.right * effectiveMagnetismRange);
        }

        public void OnLevelGoalReached ()
        {
            active = false;
            setLevitatorState(false);
            StartCoroutine(lineAnimation(false));
        }

        public void OnDeathReset ()
        {
            setLevitatorState(false);

            // clear all current particles too
            var emptyParticles = new ParticleSystem.Particle[LevitatorParticles.particleCount];
            LevitatorParticles.SetParticles(emptyParticles);
        }

        void setLevitatorState (bool on)
        {
            if (LevitatorParticles.isPlaying == on) return;

            if (on) LevitatorParticles.Play();
            else LevitatorParticles.Stop();
        }

        Vector3 getLineMeetupPoint ()
        {
            return new Vector3(LevitatorLineConnector.position.x, LeftAnchor.position.y, LeftAnchor.position.z);
        }

        Vector3 magnetRangeMidpoint ()
        {
            return new Vector3
            (
                Mathf.Lerp(LeftAnchor.position.x, RightAnchor.position.x, Driver.Mover.PositionOnLine),
                LeftAnchor.position.y + HorizontalLineVerticalOffset,
                LeftAnchor.position.z
            );
        }

        IEnumerator lineAnimation (bool loadingIn)
        {
            float start = loadingIn ? 0 : 1, end = loadingIn ? 1 : 0;

            Vector3
                horizontalLineLeftTarget = LeftAnchor.position + Vector3.left * effectiveMagnetismRange + Vector3.up * HorizontalLineVerticalOffset,
                horizontalLineRightTarget = RightAnchor.position + Vector3.right * effectiveMagnetismRange  + Vector3.up * HorizontalLineVerticalOffset,
                horizontalLineMidpoint = (horizontalLineLeftTarget + horizontalLineRightTarget) / 2;

            Func<Vector3>
                verticalLineBottomTarget = () => LevitatorLineConnector.position,
                verticalLineTopTarget = getLineMeetupPoint,
                verticalLineMidpoint = () => (verticalLineBottomTarget() + verticalLineTopTarget()) / 2;

            Func<Vector3>
                magnetLineLeftTarget = () => magnetRangeMidpoint() + Vector3.left * effectiveMagnetismRange,
                magnetLineRightTarget = () => magnetRangeMidpoint() + Vector3.right * effectiveMagnetismRange;

            void lerpLines (float lerp)
            {
                HorizontalLine.SetPosition(0, Vector3.Lerp(horizontalLineMidpoint, horizontalLineLeftTarget, lerp));
                HorizontalLine.SetPosition(1, Vector3.Lerp(horizontalLineMidpoint, horizontalLineRightTarget, lerp));
                VerticalLine.SetPosition(0, Vector3.Lerp(verticalLineMidpoint(), verticalLineBottomTarget(), lerp));
                VerticalLine.SetPosition(1, Vector3.Lerp(verticalLineMidpoint(), verticalLineTopTarget(), lerp));
                MagnetRangeLine.SetPosition(0, Vector3.Lerp(magnetRangeMidpoint(), magnetLineLeftTarget(), lerp));
                MagnetRangeLine.SetPosition(1, Vector3.Lerp(magnetRangeMidpoint(), magnetLineRightTarget(), lerp));
            }

            LineTransition.AttachMonoBehaviour(this);
            LineTransition.FlashFromTo(start, end);
            while (LineTransition.Transitioning)
            {
                lerpLines(LineTransition.Value);
                yield return null;
            };

            lerpLines(end);

            animatingLine = false;
        }
    }
}
