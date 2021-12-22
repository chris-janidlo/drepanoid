using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drepanoid
{
    public class QualitySetter : MonoBehaviour
    {
        public float TargetFps = 100;
        public float PostProcessStackFillRateMultiplier;

        public PostProcessLayer PostProcessLayer;

        void Start ()
        {
            setQuality();
        }

        void setQuality ()
        {
            // from https://stackoverflow.com/a/52063081/5931898

            int shaderLevel = SystemInfo.graphicsShaderLevel,
                cpuCount = SystemInfo.processorCount,
                vram = SystemInfo.graphicsMemorySize;

            int fillRate =
                shaderLevel < 10 ? 1000 :
                shaderLevel < 20 ? 1300 :
                shaderLevel < 30 ? 2000 :
                3000;

            if (cpuCount >= 6)
            {
                fillRate *= 3;
            }
            else if (cpuCount >= 3)
            {
                fillRate *= 2;
            }

            if (vram >= 512)
            {
                fillRate *= 2;
            }
            else if (vram <= 128)
            {
                fillRate /= 2;
            }

            float neededFillRate = (Screen.height * Screen.width + 120000f) * (TargetFps / 1000000f);
            PostProcessLayer.enabled = fillRate > neededFillRate * PostProcessStackFillRateMultiplier;
        }
    }
}
