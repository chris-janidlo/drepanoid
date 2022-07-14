using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

namespace Drepanoid
{
    public class PaddleGuardRail : MonoBehaviour
    {
        public int Extent;

        public float AnimationWaitTime;
        public TransitionableFloat AnimationTransition;

        public Paddle Paddle;
        public LineRenderer Line;


        // use Vector2 here because the guard rail is on a different z plane than the paddle
        public Vector2 Center => (Vector2) transform.position;

        public Vector2 LeftEdge => Center + Vector2.left * Extent;
        
        public Vector2 RightEdge => Center + Vector2.right * Extent;

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

                // assume each segment is one tile(=1u) wide
                float paddleHalfWidth = Paddle.Segments.Count / 2f;

                // these have to be in local space because the line is in local space
                Vector2
                    lineLeftEdge = Vector2.left * (Extent + paddleHalfWidth),
                    lineRightEdge = Vector2.right * (Extent + paddleHalfWidth);

                float start = opening ? 0 : 1, end = opening ? 1 : 0;

                void lerpLines (float lerp)
                {
                    Line.SetPosition(0, Vector2.Lerp(Vector2.zero, lineLeftEdge, lerp));
                    Line.SetPosition(1, Vector2.Lerp(Vector2.zero, lineRightEdge, lerp));
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
    }
}
