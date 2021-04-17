using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PaddleSegment : MonoBehaviour
{
    public float BounceSpeed;
    [Range(-90, 90)]
    public float BounceAngle;

    [Range(0, 1)]
    public float OriginalXSpeedOfBallRetainedOnBounce;

    public float InheritSpeedAngleMultiplier, InheritSpeedBounceMultiplier;

    public TranslationMover Mover;

    void OnCollisionEnter2D (Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            var inheritSpeedAngle = Mover.Velocity * InheritSpeedAngleMultiplier;
            var trueAngle = Mathf.Clamp(BounceAngle + inheritSpeedAngle, -180, 180);

            var trueSpeed = BounceSpeed + Mathf.Abs(Mover.Velocity) * InheritSpeedBounceMultiplier;

            var newVelocity = trueSpeed * angleToVector(trueAngle);
            newVelocity.x += ball.Velocity.x * OriginalXSpeedOfBallRetainedOnBounce;

            ball.Bounce(newVelocity, Vector2Int.up);
        }
    }

    Vector2 angleToVector (float angle)
    {
        var rad = -angle * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
    }
}
