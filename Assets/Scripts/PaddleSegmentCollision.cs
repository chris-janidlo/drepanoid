using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleSegmentCollision
{
    public readonly Collision2D Collision;
    public readonly PaddleSegment PaddleSegment;
    public readonly Ball Ball;

    public readonly bool IsReflectionBounce;

    public PaddleSegmentCollision (Collision2D collision, Ball ball)
    {
        var paddle = collision.otherCollider.GetComponent<PaddleSegment>();
        if (paddle == null)
        {
            throw new System.ArgumentException("can only make a PaddleSegmentCollision with a collision of a ball onto a paddle segment");
        }

        Collision = collision;
        PaddleSegment = paddle;
        Ball = ball;

        IsReflectionBounce = Collision.transform.position.y < PaddleSegment.transform.position.y + PaddleSegment.BounceCutoffY;
    }
}
