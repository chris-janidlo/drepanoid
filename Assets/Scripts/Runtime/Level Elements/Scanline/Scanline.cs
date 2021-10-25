using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class Scanline : MonoBehaviour
    {
        public float VerticalAcceleration;
        [Tooltip("If the ball's vertical speed is lower than this while in contact with the scanline, the scanline will apply a drag acceleration to both the ball's x and y velocity axes. Otherwise, the scanline only applies its drag vertically.")]
        public float FallCatcherThreshold;
        public float DragCoefficient;

        public Transform LeftWall, RightWall;
        public Vector2Variable CameraTrackingPosition;

        void FixedUpdate ()
        {
            transform.position = new Vector3
            (
                Mathf.Lerp(LeftWall.position.x, RightWall.position.x, Driver.Mover.PositionOnLine),
                CameraTrackingPosition.Value.y,
                0
            );
        }

        void OnTriggerStay2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            Vector2 dragAcceleration = ball.Velocity.y < FallCatcherThreshold
                ? DragCoefficient * (ball.Velocity.sqrMagnitude / 2) * -ball.Velocity.normalized
                : DragCoefficient * (ball.Velocity.y * ball.Velocity.y / 2) * Mathf.Sign(ball.Velocity.y) * Vector2.down;

            ball.Velocity += (VerticalAcceleration * Vector2.up + dragAcceleration) * Time.deltaTime;
        }
    }
}
