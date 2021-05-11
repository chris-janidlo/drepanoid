using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Collider2D))]
public class PaddleSegment : MonoBehaviour
{
    public float BounceSpeed;
    [Range(-90, 90)]
    public float BounceAngle;

    [Tooltip("Any collisions that happen below this Y value, which is in local space, are not considered bounces")]
    public float BounceCutoffY;
    public bool ReflectDownwardWhenNotBouncing;

    [Range(0, 1)]
    public float OriginalXSpeedOfBallRetainedOnBounce;

    public float InheritSpeedAngleMultiplier, InheritSpeedBounceMultiplier;

    public TranslationMover Mover;
    public PaddleSegmentAnimator Animator;

    void OnCollisionEnter2D (Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        if (collision.transform.position.y < transform.position.y + BounceCutoffY)
        {
            reflectionBounce(ball, collision);
        }
        else
        {
            paddleBounce(ball);
        }
    }

    void reflectionBounce (Ball ball, Collision2D collision)
    {
        var reflectNormal = ReflectDownwardWhenNotBouncing ? Vector2.down : -collision.GetContact(0).normal;
        var newVelocity = Vector2.Reflect(ball.Velocity, reflectNormal);
        ball.Bounce(newVelocity, Vector2Int.down);
    }

    void paddleBounce (Ball ball)
    {
        var inheritAngleDirection = MathfExtra.TernarySign(Mover.LineRightEdge.position.x - Mover.LineLeftEdge.position.x); // flip the angle if the mover goes from right to left, or don't inherit any angle if the mover goes up and down
        var inheritSpeedAngle = Mover.Velocity * InheritSpeedAngleMultiplier * inheritAngleDirection;
        var trueAngle = Mathf.Clamp(BounceAngle + inheritSpeedAngle, -180, 180);

        var trueSpeed = BounceSpeed + Mathf.Abs(Mover.Velocity) * InheritSpeedBounceMultiplier;

        var newVelocity = trueSpeed * angleToVector(trueAngle);
        newVelocity.x += ball.Velocity.x * OriginalXSpeedOfBallRetainedOnBounce;

        ball.Bounce(newVelocity, Vector2Int.up);
        Animator.OnBounce();
    }

    Vector2 angleToVector (float angle)
    {
        var rad = -angle * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
    }
}
