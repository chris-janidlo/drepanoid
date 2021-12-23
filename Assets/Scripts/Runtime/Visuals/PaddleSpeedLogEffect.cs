using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class PaddleSpeedLogEffect : MonoBehaviour
    {
        public SetTextOptions TextOptions;
        public Vector2Variable BallVelocity;

        string baseText;
        float lastLoggedPaddleVelocity = -1;
        float lastLoggedBallSpeed = -1;

        IEnumerator Start ()
        {
            baseText = TextOptions.Text;

            while (true)
            {
                float currentPaddleVelocity = Mathf.Abs(Driver.Mover.Velocity);
                float currentBallSpeed = BallVelocity.Value.magnitude;
                if (currentPaddleVelocity == lastLoggedPaddleVelocity && currentBallSpeed == lastLoggedBallSpeed)
                {
                    yield return null;
                    continue;
                }

                TextOptions.Text = string.Format(baseText, Mathf.RoundToInt(currentPaddleVelocity * 100), currentBallSpeed);
                yield return Driver.Text.SetText(TextOptions);

                lastLoggedPaddleVelocity = currentPaddleVelocity;
                lastLoggedBallSpeed = currentBallSpeed;
            }
        }
    }
}
