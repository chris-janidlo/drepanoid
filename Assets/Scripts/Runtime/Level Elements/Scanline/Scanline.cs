using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class Scanline : MonoBehaviour
    {
        public float VerticalAcceleration, InheritSpeedMultiplier;
        public float DragCoefficient, FallCatcherDragCoefficient;
        [Tooltip("If the ball's vertical speed is lower than this while in contact with the scanline, the scanline will apply a heightened drag acceleration to both the ball's x and y velocity axes. Otherwise, the scanline only applies a lower drag to the y axis.")]
        public float FallCatcherThreshold;

        public float BallMagnetismRange;
        [Range(0, 1)]
        public float BallMagnetismAmount;

        public Transform LeftWall, RightWall;
        public Vector2Variable CameraTrackingPosition;

        void FixedUpdate ()
        {
            float lerpedLinePosition = Mathf.Lerp(LeftWall.position.x, RightWall.position.x, Driver.Mover.PositionOnLine);
            float fullyMagnetizedLinePosition = Mathf.Clamp(CameraTrackingPosition.Value.x, lerpedLinePosition - BallMagnetismRange, lerpedLinePosition + BallMagnetismRange);

            transform.position = new Vector3
            (
                Mathf.Lerp(lerpedLinePosition, fullyMagnetizedLinePosition, BallMagnetismAmount),
                CameraTrackingPosition.Value.y,
                0
            );
        }

        void OnTriggerStay2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            Vector2 dragAcceleration = ball.Velocity.y < FallCatcherThreshold
                ? FallCatcherDragCoefficient * (ball.Velocity.sqrMagnitude / 2) * -ball.Velocity.normalized
                : DragCoefficient * (ball.Velocity.y * ball.Velocity.y / 2) * Mathf.Sign(ball.Velocity.y) * Vector2.down;

            ball.Velocity += (VerticalAcceleration * Vector2.up + dragAcceleration) * Time.deltaTime;
            ball.Velocity += Driver.Mover.Velocity * InheritSpeedMultiplier * Vector2.right;
        }
    }
}
