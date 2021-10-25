using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [Serializable]
    public class PaddleSegmentBounceStats
    {
        public float BounceSpeed;
        [Range(-90, 90)]
        public float BounceAngle;
        [Range(0, 1)]
        public float OriginalXSpeedOfBallRetainedOnBounce;
        public float InheritSpeedAngleMultiplier, InheritSpeedBounceMultiplier;

        public static PaddleSegmentBounceStats Average (IEnumerable<PaddleSegmentBounceStats> blocks)
        {
            return new PaddleSegmentBounceStats
            {
                BounceSpeed = blocks.Average(b => b.BounceSpeed),
                BounceAngle = blocks.Average(b => b.BounceAngle),
                OriginalXSpeedOfBallRetainedOnBounce = blocks.Average(b => b.OriginalXSpeedOfBallRetainedOnBounce),
                InheritSpeedAngleMultiplier = blocks.Average(b => b.InheritSpeedAngleMultiplier),
                InheritSpeedBounceMultiplier = blocks.Average(b => b.InheritSpeedBounceMultiplier)
            };
        }
    }
}