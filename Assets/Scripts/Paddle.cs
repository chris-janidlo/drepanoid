using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drepanoid.Drivers;
using crass;

namespace Drepanoid
{
    public class Paddle : MonoBehaviour
    {
        public List<PaddleSegment> Segments;
        public Transform LineLeftEdge, LineRightEdge;

        PaddleCollision lastBounceThisFrame, penultimateBounceThisFrame, reflectionThisFrame;

        void Start ()
        {
            foreach (var segment in Segments)
            {
                segment.Initialize(this);
            }
        }

        void FixedUpdate ()
        {
            transform.position = Vector3.Lerp(LineLeftEdge.position, LineRightEdge.position, Driver.Mover.PositionOnLine);

            if (lastBounceThisFrame != null)
            {
                bounce();
            }
            else if (reflectionThisFrame != null)
            {
                reflect();
            }

            lastBounceThisFrame = null;
            penultimateBounceThisFrame = null;
            reflectionThisFrame = null;
        }

        public void RegisterBounce (PaddleCollision collision)
        {
            penultimateBounceThisFrame = lastBounceThisFrame;
            lastBounceThisFrame = collision;
        }

        public void RegisterReflection (PaddleCollision collision)
        {
            reflectionThisFrame = collision;
        }

        void reflect ()
        {
            Ball ball = reflectionThisFrame.Ball;
            Vector2 newVelocity = Vector2.Reflect(ball.Velocity, Vector2.down);
            ball.Bounce(newVelocity, Vector2Int.down, false);
        }

        void bounce ()
        {
            List<PaddleSegmentBounceStats> statBlocks = new List<PaddleSegmentBounceStats> { lastBounceThisFrame.PaddleSegment.BounceStats };

            if (penultimateBounceThisFrame != null && collisionsWereNextToEachOther())
            {
                statBlocks.Add(penultimateBounceThisFrame.PaddleSegment.BounceStats);
            }

            PaddleSegmentBounceStats bounceStats = PaddleSegmentBounceStats.Average(statBlocks);

            float inheritAngleDirection = MathfExtra.TernarySign(LineRightEdge.position.x - LineLeftEdge.position.x); // flip the angle if the mover goes from right to left, or don't inherit any angle if the mover goes up and down
            float inheritSpeedAngle = Driver.Mover.Velocity * bounceStats.InheritSpeedAngleMultiplier * inheritAngleDirection;
            float exitAngleOffPaddle = Mathf.Clamp(bounceStats.BounceAngle + inheritSpeedAngle, -180, 180);

            Ball ball = lastBounceThisFrame.Ball;
            float speed = bounceStats.BounceSpeed + Mathf.Abs(Driver.Mover.Velocity) * bounceStats.InheritSpeedBounceMultiplier;
            Vector2 newVelocity = speed * angleToVector(exitAngleOffPaddle);
            newVelocity.x += ball.Velocity.x * bounceStats.OriginalXSpeedOfBallRetainedOnBounce;

            ball.Bounce(newVelocity, Vector2Int.up, true);
        }

        bool collisionsWereNextToEachOther ()
        {
            PaddleSegment lastSegment = lastBounceThisFrame.PaddleSegment,
                penultimateSegment = penultimateBounceThisFrame.PaddleSegment;
            return Mathf.Abs(Segments.IndexOf(lastSegment) - Segments.IndexOf(penultimateSegment)) == 1;
        }

        Vector2 angleToVector (float angle)
        {
            float rad = -angle * Mathf.Deg2Rad;
            return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
        }
    }
}