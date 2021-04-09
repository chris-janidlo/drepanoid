using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class TranslationMover : MonoBehaviour
{
    public float Acceleration;
    public Transform LineLeftEdge, LineRightEdge;
    public FloatVariable MovementAxis;

    float velocity, positionOnLine = 0.5f;

    void FixedUpdate ()
    {
        if (MovementAxis.Value == 0)
        {
            velocity = 0;
        }
        else
        {
            velocity += Acceleration * MovementAxis.Value * Time.deltaTime;
            positionOnLine += velocity * Time.deltaTime;
            positionOnLine = Mathf.Clamp(positionOnLine, 0, 1);
        }

        transform.position = Vector3.Lerp(LineLeftEdge.position, LineRightEdge.position, positionOnLine);
    }
}
