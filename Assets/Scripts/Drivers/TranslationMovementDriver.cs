using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace Drepanoid.Drivers
{
    public class TranslationMovementDriver : MonoBehaviour
    {
        public float Velocity { get; private set; }
        public float PositionOnLine { get; private set; } = 0.5f;

        public float MaxVelocity, AccelerationTime;
        public FloatVariable MovementAxis;

        void FixedUpdate ()
        {
            if (MovementAxis.Value == 0 || Mathf.Sign(MovementAxis.Value) != Mathf.Sign(Velocity))
            {
                Velocity = 0;
            }

            if (MovementAxis.Value != 0)
            {
                Velocity += MaxVelocity / AccelerationTime * MovementAxis.Value * Time.deltaTime;
                Velocity = Mathf.Clamp(Velocity, -MaxVelocity, MaxVelocity);

                PositionOnLine += Velocity * Time.deltaTime;
                PositionOnLine = Mathf.Clamp(PositionOnLine, 0, 1);

                if (PositionOnLine == 0 && Velocity < 0 || PositionOnLine == 1 && Velocity > 0)
                {
                    Velocity = 0;
                }
            }
        }
    }
}