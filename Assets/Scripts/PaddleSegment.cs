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

    [Range(0, 1)]
    public float OriginalXSpeedOfBallRetainedOnBounce;

    public float InheritSpeedAngleMultiplier, InheritSpeedBounceMultiplier;

    public TransitionableFloat MoveLagTransition;

    public TranslationMover Mover;
    public PaddleSegmentAnimator Animator;

    Vector3 startingLocalPosition, positionAtStartOfLag;
    int previousMoveDirection;

    void Start ()
    {
        MoveLagTransition.AttachMonoBehaviour(this);
        MoveLagTransition.Value = 1;

        startingLocalPosition = transform.localPosition;
    }

    void Update ()
    {
        var moveDirection = ternarySign(Mover.Velocity);
        if (moveDirection != previousMoveDirection)
        {
            previousMoveDirection = moveDirection;

            if (moveDirection != 0)
            {
                positionAtStartOfLag = transform.position;
                MoveLagTransition.FlashFromTo(0, 1);
            }
        }

        transform.position = Vector3.Lerp(positionAtStartOfLag, transform.parent.position + startingLocalPosition, MoveLagTransition.Value);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        if (collision.transform.position.y < transform.position.y + BounceCutoffY)
        {
            ball.Velocity = new Vector2(ball.Velocity.x, 0);
            return;
        }

        var inheritSpeedAngle = Mover.Velocity * InheritSpeedAngleMultiplier;
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

    int ternarySign (float value)
    {
        return value == 0 ? 0 : Math.Sign(value);
    }
}
