using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    void OnCollisionEnter2D (Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        // negative since normal is from the perspective of the ball
        var normal = -collision.GetContact(0).normal;
        var resultingSpeed = Vector2.Reflect(ball.Velocity, normal);

        var cardinalNormal = Mathf.Abs(normal.x) > Mathf.Abs(normal.y)
            ? new Vector2Int(Math.Sign(normal.x), 0)
            : new Vector2Int(0, Math.Sign(normal.y));

        ball.Bounce(resultingSpeed, cardinalNormal);
    }
}
