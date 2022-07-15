using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

namespace Drepanoid
{
    public class PaddleGuardRail : MonoBehaviour
    {
        [Min(0)]
        public int Extent;

        public float AnimationWaitTime;
        public TransitionableFloat AnimationTransition;

        public Paddle Paddle;
        public LineRenderer Line;


        // use Vector2 here because the guard rail is on a different z plane than the paddle
        public Vector2 Center => (Vector2) transform.position;

        public Vector2 LeftEdge => Center + Vector2.left * Extent;
        public Vector2 RightEdge => Center + Vector2.right * Extent;

        // assume each segment is one tile(=1u) wide
        private float paddleHalfWidth => Paddle.Segments.Count / 2f;
        private Vector3 visualLeftEdge => transform.TransformPoint(Vector2.left * (Extent + paddleHalfWidth));
        private Vector3 visualRighttEdge => transform.TransformPoint(Vector2.right * (Extent + paddleHalfWidth));

        void Start ()
        {
            if (Line.positionCount != 2)
            {
                throw new System.InvalidOperationException("Line must have 2 positions");
            }

            AnimationTransition.AttachMonoBehaviour(this);

            animateLine(true);
        }

        public void OnLevelGoalReached ()
        {
            animateLine(false);
        }

        void animateLine (bool opening)
        {
            IEnumerator animation ()
            {
                yield return new WaitForSeconds(AnimationWaitTime);

                float start = opening ? 0 : 1, end = opening ? 1 : 0;

                void lerpLines (float lerp)
                {
                    Line.SetPosition(0, Vector3.Lerp(transform.position, visualLeftEdge, lerp));
                    Line.SetPosition(1, Vector3.Lerp(transform.position, visualRighttEdge, lerp));
                }

                AnimationTransition.FlashFromTo(start, end);
                while (AnimationTransition.Transitioning)
                {
                    lerpLines(AnimationTransition.Value);
                    yield return null;
                }

                lerpLines(end);
            }

            StartCoroutine(animation());
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(visualLeftEdge, visualRighttEdge);
        }
#endif // UNITY_EDITOR
    }
}
