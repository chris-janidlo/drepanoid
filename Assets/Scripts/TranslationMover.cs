using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class TranslationMover : MonoBehaviour
{
    public float Velocity { get; private set; }

    public float Acceleration;
    public Transform LineLeftEdge, LineRightEdge;
    public FloatVariable MovementAxis;

    float positionOnLine = 0.5f;

    void FixedUpdate ()
    {
        if (MovementAxis.Value == 0 || Mathf.Sign(MovementAxis.Value) != Mathf.Sign(Velocity))
        {
            Velocity = 0;
        }

        if (MovementAxis.Value != 0)
        {
            Velocity += Acceleration * MovementAxis.Value * Time.deltaTime;
            positionOnLine += Velocity * Time.deltaTime;
            positionOnLine = Mathf.Clamp(positionOnLine, 0, 1);
        }

        transform.position = Vector3.Lerp(LineLeftEdge.position, LineRightEdge.position, positionOnLine);
    }
}
