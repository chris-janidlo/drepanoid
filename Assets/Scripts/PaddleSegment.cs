using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PaddleSegment : MonoBehaviour
{
    public PaddleSegmentBounceStats BounceStats;

    [Tooltip("Any collisions that happen below this Y value, which is in local space, are not considered bounces")]
    public float BounceCutoffY;
    public bool ReflectDownwardWhenNotBouncing;

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

        var paddleCollision = new PaddleSegmentCollision(collision, ball);
        paddle.RegisterCollision(paddleCollision);
        if (!paddleCollision.IsReflectionBounce) Animator.OnBounce();
    }
}
