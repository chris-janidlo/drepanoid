using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Ball : MonoBehaviour
{
    public Vector2 Velocity { get; private set; }

    public float Gravity;
    [Tooltip("Abstract constant that contains every variable in the drag equation that isn't velocity (so it includes coefficient of drag, reference area, and fluid density, which are all held constant throughout the game)")]
    public float DragCoefficient;

    public VoidEvent BallDied;

    void FixedUpdate ()
    {
        Velocity += Vector2.down * Gravity * Time.deltaTime;

        var dragAcceleration = DragCoefficient * (Velocity.sqrMagnitude / 2) * -Velocity.normalized; // drag equation
        Velocity += dragAcceleration * Time.deltaTime;

        transform.position += (Vector3) Velocity * Time.deltaTime;
    }

    public void SetVelocity (Vector2 value)
    {
        // TODO: calculate spin
        Velocity = value;
    }

    public void Kill ()
    {
        BallDied.Raise();
        Destroy(gameObject);
    }
}
