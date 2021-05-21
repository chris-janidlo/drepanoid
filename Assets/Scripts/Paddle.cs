using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Paddle : MonoBehaviour
{
    public List<PaddleSegment> Segments;
    public TranslationMover Mover;

    PaddleSegmentCollision lastCollisionThisFrame, penultimateCollisionThisFrame;

    void Start ()
    {
        foreach (var segment in Segments)
        {
            segment.Initialize(this);
        }
    }

    void FixedUpdate ()
    {
        if (lastCollisionThisFrame == null) return;

        if (lastCollisionThisFrame.IsReflectionBounce)
        {
            reflectionBounce();
        }
        else
        {
            paddleBounce();
        }

        lastCollisionThisFrame = null;
        penultimateCollisionThisFrame = null;
    }

    public void RegisterCollision (PaddleSegmentCollision collision)
    {
        penultimateCollisionThisFrame = lastCollisionThisFrame;
        lastCollisionThisFrame = collision;
    }

    void reflectionBounce ()
    {
        var ball = lastCollisionThisFrame.Ball;
        var reflectNormal =  lastCollisionThisFrame.PaddleSegment.ReflectDownwardWhenNotBouncing
            ? Vector2.down
            : -lastCollisionThisFrame.Collision.GetContact(0).normal;
        var newVelocity = Vector2.Reflect(ball.Velocity, reflectNormal);
        ball.Bounce(newVelocity, Vector2Int.down);
    }

    void paddleBounce ()
    {
        List<PaddleSegmentBounceStats> statBlocks = new List<PaddleSegmentBounceStats>();
        statBlocks.Add(lastCollisionThisFrame.PaddleSegment.BounceStats);

        if (penultimateCollisionThisFrame != null && collisionsWereNextToEachOther())
        {
            statBlocks.Add(penultimateCollisionThisFrame.PaddleSegment.BounceStats);
        }

        PaddleSegmentBounceStats bounceStats = PaddleSegmentBounceStats.Average(statBlocks);

        var inheritAngleDirection = MathfExtra.TernarySign(Mover.LineRightEdge.position.x - Mover.LineLeftEdge.position.x); // flip the angle if the mover goes from right to left, or don't inherit any angle if the mover goes up and down
        var inheritSpeedAngle = Mover.Velocity * bounceStats.InheritSpeedAngleMultiplier * inheritAngleDirection;
        var trueAngle = Mathf.Clamp(bounceStats.BounceAngle + inheritSpeedAngle, -180, 180);

        var ball = lastCollisionThisFrame.Ball;
        var trueSpeed = bounceStats.BounceSpeed + Mathf.Abs(Mover.Velocity) * bounceStats.InheritSpeedBounceMultiplier;
        var newVelocity = trueSpeed * angleToVector(trueAngle);
        newVelocity.x += ball.Velocity.x * bounceStats.OriginalXSpeedOfBallRetainedOnBounce;

        ball.Bounce(newVelocity, Vector2Int.up);
    }

    bool collisionsWereNextToEachOther ()
    {
        PaddleSegment lastSegment = lastCollisionThisFrame.PaddleSegment,
            penultimateSegment = penultimateCollisionThisFrame.PaddleSegment;
        return Mathf.Abs(Segments.IndexOf(lastSegment) - Segments.IndexOf(penultimateSegment)) == 1;
    }

    Vector2 angleToVector (float angle)
    {
        var rad = -angle * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
    }
}
