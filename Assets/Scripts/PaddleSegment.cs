using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [RequireComponent(typeof(Collider2D))]
    public class PaddleSegment : MonoBehaviour
    {
        public PaddleSegmentBounceStats BounceStats;
        public PaddleSegmentAnimator Animator;

        Paddle paddle;

        public void Initialize (Paddle paddle)
        {
            this.paddle = paddle;
        }

        void OnCollisionEnter2D (Collision2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            paddle.RegisterBounce(new PaddleCollision { PaddleSegment = this, Ball = ball, Collision = collision });
            Animator.OnBounce();
        }
    }
}