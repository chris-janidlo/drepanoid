using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Text Transformers/Speed Stats", fileName = "newSpeedStatsTransformer.asset")]
    public class SpeedStatsTransformer : TextTransformer
    {
        public string BallSpeedFormat, PaddleSpeedFormat;
        public Vector2Variable BallVelocity;

        public override string Transform (string input)
        {
            float currentBallSpeed = BallVelocity.Value.magnitude;
            string formattedBallSpeed = string.Format(BallSpeedFormat, currentBallSpeed);

            float currentPaddleSpeed = Mathf.Abs(Driver.Mover.Velocity);
            string formattedPaddleSpeed = string.Format(PaddleSpeedFormat, Mathf.RoundToInt(currentPaddleSpeed * 100));

            return input
                .Replace("$ballSpeed", formattedBallSpeed)
                .Replace("$paddleSpeed", formattedPaddleSpeed);
        }
    }
}
