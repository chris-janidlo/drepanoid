using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using crass;

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
            int moveDirection = MathfExtra.TernarySign(MovementAxis.Value);

            if (moveDirection == 0 || PositionOnLine == 0 && moveDirection == -1 || PositionOnLine == 1 && moveDirection == 1)
            {
                Velocity = 0;
                return;
            }

            float accelerationThisFrame = MaxVelocity / AccelerationTime * MovementAxis.Value * Time.deltaTime;
            float unclampedVelocity = moveDirection != Mathf.Sign(Velocity)
                ? accelerationThisFrame
                : Velocity + accelerationThisFrame;

            Velocity = Mathf.Clamp(unclampedVelocity, -MaxVelocity, MaxVelocity);
            PositionOnLine = Mathf.Clamp(PositionOnLine + Velocity * Time.deltaTime, 0, 1);
        }
    }
}
