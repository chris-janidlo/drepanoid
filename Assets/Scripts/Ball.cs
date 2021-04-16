using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

public class Ball : MonoBehaviour
{
    public Vector2 Velocity { get; private set; }

    public float Gravity;
    [Tooltip("Abstract constant that contains every variable in the drag equation that isn't velocity (so it includes coefficient of drag, reference area, and fluid density, which are all held constant throughout the game)")]
    public float DragCoefficient;

    public TransitionableFloat SpawnScaleTransition;
    public int SpawnScaleTransitionRounding;

    public float BounceSpinMultiplier;
    [Range(0, 1)]
    public float AngularDrag;

    public float KillFloorY;
    public VoidEvent BallDied;

    float angularVelocity; // positive = clockwise, negative = counter-clockwise

    void Start ()
    {
        SpawnScaleTransition.AttachMonoBehaviour(this);
        SpawnScaleTransition.FlashFromTo(0, 1);
    }

    void Update ()
    {
        transform.localScale = Vector3.one * (Mathf.Round(SpawnScaleTransition.Value * SpawnScaleTransitionRounding) / SpawnScaleTransitionRounding);
    }

    void FixedUpdate ()
    {
        move();
        spin();

        if (transform.position.y < KillFloorY) Kill();
    }

    /// <summary>
    /// Change the ball's velocity and spin as a result of bouncing off of something like a paddle or wall.
    /// </summary>
    /// <param name="resultingVelocity">How fast, and in what direction, the ball should be travelling after this collision.</param>
    /// <param name="cardinalCollisionNormal">The cardinal direction (ie up, down, left, right) that most closely matches the normal of the collision.</param>
    public void Bounce (Vector2 resultingVelocity, Vector2Int cardinalCollisionNormal)
    {
        var validNormals = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        if (!validNormals.Contains(cardinalCollisionNormal))
        {
            throw new ArgumentException($"axisAlignedCollisionNormal must be one of {string.Join(", ", validNormals)} - {cardinalCollisionNormal} is invalid");
        }

        if (cardinalCollisionNormal != Vector2Int.up) throw new NotImplementedException("TODO: implement bouncing in other directions");

        angularVelocity += resultingVelocity.x * BounceSpinMultiplier;

        Velocity = resultingVelocity;
    }

    public void Kill ()
    {
        BallDied.Raise();
        Destroy(gameObject);
    }

    void move ()
    {
        Velocity += Vector2.down * Gravity * Time.deltaTime;

        var dragAcceleration = DragCoefficient * (Velocity.sqrMagnitude / 2) * -Velocity.normalized; // drag equation
        Velocity += dragAcceleration * Time.deltaTime;

        transform.position += (Vector3) Velocity * Time.deltaTime;
    }

    void spin ()
    {
        transform.Rotate(angularVelocity * Vector3.back * Time.deltaTime);

        if (Mathf.Abs(angularVelocity) >= Time.deltaTime)
        {
            angularVelocity -= angularVelocity * AngularDrag * Time.deltaTime;
        }
        else
        {
            angularVelocity = 0;
        }
    }
}
