using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityAtoms.BaseAtoms;
using crass;

namespace Drepanoid
{
    public class DeathGlitch : MonoBehaviour
    {
        // visuals are based off the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

        [Header("Visual Effect Settings")]
        public float DeathResetTime;
        public AnimationCurve GlitchIntensityByTimeSinceDeath;

        [Range(0, 1), Tooltip("How \"un-sticky\" the colors of the block-damage effect's noise texture are - the higher the value, the less likely that neighboring pixels are the same color")]
        public float Noisiness = 0.11f;

        [Range(0, 1)]
        public float NoiseUpdateChancePerFrame = 0.7f;

        [Tooltip("How many frames between each time the trash frame is updated")]
        public int
            TrashFrame1UpdateInterval = 13,
            TrashFrame2UpdateInterval = 73;

        [Header("References")]
        public Shader Shader;

        public PostProcessProfile PostProcessProfile;
        public RetriggerFilter RetriggerFilter;

        public BoolVariable DeathGlitchEffectIsOn;
        public VoidEvent DeathReset;
        public FloatVariable MusicTrackPosition;

        Material material;

        float currentIntensity;
        Texture2D noiseTexture;
        RenderTexture trashFrame1, trashFrame2;

        void Start ()
        {
            resetEffect();

            material = new Material(Shader) { hideFlags = HideFlags.DontSave };

            noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            updateNoise();

            trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0) { hideFlags = HideFlags.DontSave };
            trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0) { hideFlags = HideFlags.DontSave };
        }

        void Update ()
        {
            if (material != null && RandomExtra.Chance(NoiseUpdateChancePerFrame)) updateNoise();
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            void updateTrash (int interval, RenderTexture texture)
            {
                if (Time.frameCount % interval == 0) Graphics.Blit(source, texture);
            }

            updateTrash(TrashFrame1UpdateInterval, trashFrame1);
            updateTrash(TrashFrame2UpdateInterval, trashFrame2);

            if (!DeathGlitchEffectIsOn.Value)
            {
                Graphics.Blit(source, destination);
                return;
            }

            material.SetFloat("_Intensity", currentIntensity);
            material.SetTexture("_NoiseTex", noiseTexture);
            material.SetTexture("_TrashTex", RandomExtra.Chance(0.5f) ? trashFrame1 : trashFrame2);

            Graphics.Blit(source, destination, material);
        }

        public void OnBallDied ()
        {
            StartCoroutine(deathRoutine());
        }

        IEnumerator deathRoutine ()
        {
            float timer = 0;
            float totalTime = GlitchIntensityByTimeSinceDeath.keys.Last().time;
            bool haveReset = false;

            DeathGlitchEffectIsOn.Value = true;
            RetriggerFilter.Active = true;
            float originalTrackPosition = MusicTrackPosition.Value;

            while (timer < totalTime)
            {
                currentIntensity = GlitchIntensityByTimeSinceDeath.Evaluate(timer);
                timer += Time.deltaTime;

                if (timer >= DeathResetTime && !haveReset)
                {
                    haveReset = true;
                    DeathReset.Raise();
                }

                yield return null;
            }

            resetEffect();
            MusicTrackPosition.Value = originalTrackPosition;
        }

        void resetEffect ()
        {
            currentIntensity = 0;
            DeathGlitchEffectIsOn.Value = false;
            RetriggerFilter.Active = false;
        }

        void updateNoise ()
        {
            static Color randomColor () => new Color(Random.value, Random.value, Random.value, Random.value);
            Color currentColor = randomColor();

            for (int y = 0; y < noiseTexture.height; y++)
            {
                for (int x = 0; x < noiseTexture.width; x++)
                {
                    if (RandomExtra.Chance(Noisiness)) currentColor = randomColor();
                    noiseTexture.SetPixel(x, y, currentColor);
                }
            }

            noiseTexture.Apply();
        }
    }
}
