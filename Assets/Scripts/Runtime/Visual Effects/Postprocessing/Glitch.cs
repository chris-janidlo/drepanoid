using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using crass;
using Random = UnityEngine.Random;

namespace Drepanoid
{
    // based off the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

    [Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Custom/Glitch", allowInSceneView: false)]
    public class Glitch : PostProcessEffectSettings
    {
        [Range(0, 1)]
        public FloatParameter Intensity = new FloatParameter { value = 0.5f };

        [Range(0, 1)]
        [Tooltip("How \"un-sticky\" the colors of the block-damage effect's noise texture are - the higher the value, the less likely that neighboring pixels are the same color")]
        public FloatParameter Noisiness = new FloatParameter { value = 0.11f };

        [Range(0, 1)]
        public FloatParameter NoiseUpdateChancePerFrame = new FloatParameter { value = 0.7f };

        [Tooltip("How many frames between each time the trash frame is updated")]
        public IntParameter
            TrashFrame1UpdateInterval = new IntParameter { value = 13 },
            TrashFrame2UpdateInterval = new IntParameter { value = 73 };
    }

    public class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        Texture2D noise;
        RenderTexture trashFrame1, trashFrame2;

        public override void Init ()
        {
            base.Init();

            noise = new Texture2D(64, 32, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            updateNoise();

            trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0) { hideFlags = HideFlags.DontSave };
            trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0) { hideFlags = HideFlags.DontSave };
        }

        public override void Render (PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));

            void updateTrash (int interval, Texture texture)
            {
                if (Time.frameCount % interval == 0) context.command.Blit(context.source, new RenderTargetIdentifier(texture));
            }

            updateTrash(settings.TrashFrame1UpdateInterval, trashFrame1);
            updateTrash(settings.TrashFrame2UpdateInterval, trashFrame2);

            if (RandomExtra.Chance(settings.NoiseUpdateChancePerFrame)) updateNoise();

            sheet.properties.SetFloat("_Intensity", settings.Intensity);
            sheet.properties.SetTexture("_NoiseTex", noise);
            sheet.properties.SetTexture("_TrashTex", RandomExtra.Chance(0.5f) ? trashFrame1 : trashFrame2);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }

        void updateNoise ()
        {
            static Color randomColor () => new Color(Random.value, Random.value, Random.value, Random.value);
            Color currentColor = randomColor();

            for (int y = 0; y < noise.height; y++)
            {
                for (int x = 0; x < noise.width; x++)
                {
                    if (RandomExtra.Chance(settings.Noisiness)) currentColor = randomColor();
                    noise.SetPixel(x, y, currentColor);
                }
            }

            noise.Apply();
        }
    }
}
