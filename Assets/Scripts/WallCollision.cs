using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    public class WallCollision : MonoBehaviour
    {
        // uses Stay instead of Enter because each level's wall is a big composite collider, and sometimes you hit one block of it and bounce into another block the same frame. would rather bounce a frame late than only bounce the first frame we're touching the wall 
        void OnCollisionStay2D (Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            var normal = -getStraightestNormal(collision); // flip it to get it from the perspective of the wall instead of the ball
            var resultingSpeed = Vector2.Reflect(ball.Velocity, normal);

            var cardinalNormal = Mathf.Abs(normal.x) > Mathf.Abs(normal.y)
                ? new Vector2Int(Math.Sign(normal.x), 0)
                : new Vector2Int(0, Math.Sign(normal.y));

            ball.Bounce(resultingSpeed, cardinalNormal);
        }

        Vector2 getStraightestNormal (Collision2D collision)
        {
            return collision.contacts
                .Select(c => c.normal)
                .OrderBy(n => Mathf.Abs(n.x) == 1 || Math.Abs(n.y) == 1 ? 0 : Mathf.Infinity)
                .ThenBy(n => Mathf.Abs(n.sqrMagnitude - 1))
                .First();
        }

        // keeping for reference sake. should never be included in committed code
        // when multiple rays are drawn, whiter rays are earlier in the contact list
        void debugCollision (Collision2D collision)
        {
            Debug.Log("new collision");

            for (int i = 0; i < collision.contacts.Length; i++)
            {
                ContactPoint2D item = collision.contacts[i];
                Debug.Log("new point");
                Debug.Log(item.point);
                Debug.Log(item.normal);
                Debug.DrawRay(item.point, item.normal * 100, new Color(1f / (i + 1), 1, 1), 1f);
            }

            Debug.Break();
        }
    }
}