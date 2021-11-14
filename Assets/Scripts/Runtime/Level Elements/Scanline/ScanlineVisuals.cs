using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

namespace Drepanoid
{
    public class ScanlineVisuals : MonoBehaviour
    {
        public float MinDistanceToTurnOnParticles, HorizontalLineVerticalOffset, SceneLoadDelayBeforeLoadingLines;

        public TransitionableFloat LineTransition;

        public Transform Levitator, LeftAnchor, RightAnchor, LevitatorLineConnector;
        public ParticleSystem LevitatorParticles;
        public LineRenderer HorizontalLine, VerticalLine;
        public Vector2Variable CameraTrackingPosition;
        public SceneTransitionHelper SceneTransitionHelper;

        bool active;

        IEnumerator Start ()
        {
            setLevitatorState(false);

            yield return new WaitForSeconds(SceneLoadDelayBeforeLoadingLines);
            yield return lineAnimation(true);
            
            yield return new WaitForSeconds(SceneTransitionHelper.LevelLoadAnimationTime - SceneLoadDelayBeforeLoadingLines - LineTransition.Time);
            active = true;
        }

        void FixedUpdate ()
        {
            if (!active) return;

            Levitator.position = transform.position;

            VerticalLine.SetPosition(0, LevitatorLineConnector.position);
            VerticalLine.SetPosition(1, getLineMeetupPoint());

            bool particlesOn = Vector2.Distance(transform.position, CameraTrackingPosition.Value) < MinDistanceToTurnOnParticles;
            setLevitatorState(particlesOn);
        }

        public void OnLevelGoalReached ()
        {
            active = false;
            setLevitatorState(false);
            StartCoroutine(lineAnimation(false));
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

        IEnumerator lineAnimation (bool loadingIn)
        {
            float start = loadingIn ? 0 : 1, end = loadingIn ? 1 : 0;

            Vector3
                horizontalLineLeftTarget = LeftAnchor.position + Vector3.up * HorizontalLineVerticalOffset,
                horizontalLineRightTarget = RightAnchor.position + Vector3.up * HorizontalLineVerticalOffset,
                horizontalLineMidpoint = (horizontalLineLeftTarget + horizontalLineRightTarget) / 2;

            Vector3
                verticalLineBottomTarget = LevitatorLineConnector.position,
                verticalLineTopTarget = getLineMeetupPoint(),
                verticalLineMidpoint = (verticalLineBottomTarget + verticalLineTopTarget) / 2;

            void lerpLines (float lerp)
            {
                HorizontalLine.SetPosition(0, Vector3.Lerp(horizontalLineMidpoint, horizontalLineLeftTarget, lerp));
                HorizontalLine.SetPosition(1, Vector3.Lerp(horizontalLineMidpoint, horizontalLineRightTarget, lerp));
                VerticalLine.SetPosition(0, Vector3.Lerp(verticalLineMidpoint, verticalLineBottomTarget, lerp));
                VerticalLine.SetPosition(1, Vector3.Lerp(verticalLineMidpoint, verticalLineTopTarget, lerp));
            }

            LineTransition.AttachMonoBehaviour(this);
            LineTransition.FlashFromTo(start, end);
            while (LineTransition.Transitioning)
            {
                lerpLines(LineTransition.Value);
                yield return null;
            };

            lerpLines(end);
        }
    }
}
