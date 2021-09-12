using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drepanoid
{
    [Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Custom/Glitch")]
    public class Glitch : PostProcessEffectSettings
    {
        [Range(0, 1)]
        [Tooltip("Currently just controls grayscale intensity as a placeholder")]
        public FloatParameter Amount = new FloatParameter { value = 0.5f };
    }

    public class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        public override void Render (PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));
            sheet.properties.SetFloat("_Amount", settings.Amount);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
