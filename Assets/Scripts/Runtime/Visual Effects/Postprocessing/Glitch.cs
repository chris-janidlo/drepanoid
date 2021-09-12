using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drepanoid
{
    [Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Custom/Glitch", allowInSceneView: false)]
    public class Glitch : PostProcessEffectSettings
    {
        [Range(0, 1)]
        [Tooltip("Determines the amount and frequency of horizontal displacement")]
        public FloatParameter ScanlineJitter = new FloatParameter { value = 0.5f };
    }

    public class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        public override void Render (PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));

            // uses a combination of the analogue glitch's scanline jitter and the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

            float jitter = settings.ScanlineJitter;

            // magic constants are from KinoGlitch, help separate overall jitter "intensity" into frequency and displacement values
            // might want to expose a control for each value instead
            float jitterFrequency = Mathf.Clamp01(1.0f - jitter * 1.2f);
            float jitterDisplacement = 0.002f + Mathf.Pow(jitter, 3) * 0.05f;

            sheet.properties.SetFloat("_ScanlineJitterFrequency", jitterFrequency);
            sheet.properties.SetFloat("_ScanlineJitterDisplacement", jitterDisplacement);

            // TODO: digital glitch

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
