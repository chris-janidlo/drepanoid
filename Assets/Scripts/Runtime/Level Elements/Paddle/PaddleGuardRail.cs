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
        public TransitionableFloat LoadInTransition, LoadOutTransition;

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

            LoadInTransition.AttachMonoBehaviour(this);
            LoadOutTransition.AttachMonoBehaviour(this);

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
                var transition = opening ? LoadInTransition : LoadOutTransition;

                // randomize which edge looks like the anchor and which edge looks like is being generated in real time
                int leftEdgeIndex = RandomExtra.Chance(.5f) ? 0 : 1;
                int rightEdgeIndex = 1 - leftEdgeIndex;

                void lerpLines (float lerp)
                {
                    Line.SetPosition(leftEdgeIndex, Vector3.Lerp(transform.position, visualLeftEdge, lerp));
                    Line.SetPosition(rightEdgeIndex, Vector3.Lerp(transform.position, visualRighttEdge, lerp));
                }

                transition.FlashFromTo(start, end);
                while (transition.Transitioning)
                {
                    lerpLines(transition.Value);
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
