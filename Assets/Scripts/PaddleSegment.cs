using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PaddleSegment : MonoBehaviour
{
    public float BounceSpeed;
    [Range(-90, 90)]
    public float BounceAngle;

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() is Ball ball)
        {
            ball.SetVelocity(BounceSpeed * angleToVector(BounceAngle));
        }
    }

    Vector2 angleToVector (float angle)
    {
        var rad = -angle * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
    }
}
