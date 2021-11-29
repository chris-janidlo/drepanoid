using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class ScanlinePhysics : MonoBehaviour
    {
        public float VerticalAcceleration, InheritSpeedMultiplier;
        public float DragCoefficient, FallCatcherDragCoefficient;
        [Tooltip("If the ball's vertical speed is lower than this while in contact with the scanline, the scanline will apply a heightened drag acceleration to both the ball's x and y velocity axes. Otherwise, the scanline only applies a lower drag to the y axis.")]
        public float FallCatcherThreshold;

        public float BallMagnetismRange;
        [Range(0, 1)]
        public float BallMagnetismAmount;
        public AnimationCurve BallMagnetismStrengthByDistanceToBall;

        public float VerticalFollowTime;

        public float MinVerticalLineLength;

        public float InitialYPosition;

        public Transform LeftAnchor, RightAnchor;
        public Vector2Variable CameraTrackingPosition;
        public BoolVariable BallIsInScanline;

        bool active = true, ballLive;
        float verticalFollowCurrentSpeed;

        void FixedUpdate ()
        {
            if (!active) return;

            Vector2 fullMagnetPos = fullStrengthMagnetismPosition();

            float magnetismStrength = BallMagnetismStrengthByDistanceToBall.Evaluate(Mathf.Abs(ballPosition().x - transform.position.x));

            Vector2 newPosition;

            if (magnetismStrength >= 1)
            {
                verticalFollowCurrentSpeed = 0;
                newPosition = fullMagnetPos;
            }
            else
            {
                newPosition = Vector2.Lerp(nonMagnetizedPosition(), fullMagnetPos, magnetismStrength);
            }

            transform.position = new Vector3
            (
                newPosition.x,
                Mathf.Min(newPosition.y, LeftAnchor.position.y - MinVerticalLineLength),
                0
            );
        }

        void OnTriggerStay2D (Collider2D collision)
        {
            if (!active) return;

            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            Vector2 dragAcceleration = ball.Velocity.y < FallCatcherThreshold
                ? FallCatcherDragCoefficient * (ball.Velocity.sqrMagnitude / 2) * -ball.Velocity.normalized
                : DragCoefficient * (ball.Velocity.y * ball.Velocity.y / 2) * Mathf.Sign(ball.Velocity.y) * Vector2.down;

            ball.Velocity += (VerticalAcceleration * Vector2.up + dragAcceleration) * Time.deltaTime;
            ball.Velocity += Driver.Mover.Velocity * InheritSpeedMultiplier * Vector2.right;

            BallIsInScanline.Value = true;
        }

        void OnTriggerExit2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            BallIsInScanline.Value = false;
        }

        public void OnLevelGoalReached ()
        {
            active = false;
        }

        public void OnBallDied ()
        {
            active = false;
            ballLive = false;
        }

        public void OnBallSpawned ()
        {
            ballLive = true;
        }

        public void OnDeathReset ()
        {
            transform.position = new Vector3(initialPosition().x, initialPosition().y, transform.position.z);
            active = true;
        }

        float lerpedLinePosition (float? t = null)
        {
            return Mathf.Lerp(LeftAnchor.position.x, RightAnchor.position.x, t ?? Driver.Mover.PositionOnLine);
        }

        Vector2 initialPosition ()
        {
            return new Vector2(lerpedLinePosition(.5f), InitialYPosition);
        }

        Vector2 ballPosition ()
        {
            return ballLive ? CameraTrackingPosition.Value : initialPosition();
        }

        Vector2 fullStrengthMagnetismPosition ()
        {
            float unlerpedMagnetPosition = Mathf.Clamp(ballPosition().x, lerpedLinePosition() - BallMagnetismRange, lerpedLinePosition() + BallMagnetismRange);
            float magnetizedLinePosition = Mathf.Lerp(lerpedLinePosition(), unlerpedMagnetPosition, BallMagnetismAmount);

            return new Vector2(magnetizedLinePosition, ballPosition().y);
        }

        Vector2 nonMagnetizedPosition ()
        {
            return new Vector2
            (
                lerpedLinePosition(),
                Mathf.SmoothDamp(transform.position.y, ballPosition().y, ref verticalFollowCurrentSpeed, VerticalFollowTime)
            );
        }
    }
}
