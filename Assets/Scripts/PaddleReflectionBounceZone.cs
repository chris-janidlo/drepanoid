using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleReflectionBounceZone : MonoBehaviour
{
    public Paddle Paddle;

    void OnCollisionEnter2D (Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        Paddle.RegisterReflection(new PaddleCollision { Ball = ball, Collision = collision });
    }
}
