using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class TranslationMover : MonoBehaviour
{
    public float Velocity { get; private set; }

    public float MaxVelocity, AccelerationTime;
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
            Velocity += (MaxVelocity / AccelerationTime) * MovementAxis.Value * Time.deltaTime;
            Velocity = Mathf.Clamp(Velocity, -MaxVelocity, MaxVelocity);

            positionOnLine += Velocity * Time.deltaTime;
            positionOnLine = Mathf.Clamp(positionOnLine, 0, 1);

            if ((positionOnLine == 0 && Velocity < 0) || (positionOnLine == 1 && Velocity > 0))
            {
                Velocity = 0;
            }
        }

        transform.position = Vector3.Lerp(LineLeftEdge.position, LineRightEdge.position, positionOnLine);
    }
}
