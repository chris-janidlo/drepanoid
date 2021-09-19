using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityAtoms.BaseAtoms;

namespace Drepanoid
{
    public class DeathGlitch : MonoBehaviour
    {
        public float DeathResetTime;
        public AnimationCurve GlitchIntensityByTimeSinceDeath;

        public PostProcessProfile PostProcessProfile;
        public BoolVariable DeathGlitchEffectIsOn;
        public VoidEvent DeathReset;

        public void OnBallDied ()
        {
            StartCoroutine(deathRoutine());
        }

        IEnumerator deathRoutine ()
        {
            float timer = 0;
            float totalTime = GlitchIntensityByTimeSinceDeath.keys.Last().time;
            bool haveReset = false;

            PostProcessProfile.TryGetSettings(out Glitch glitch);

            DeathGlitchEffectIsOn.Value = true;

            while (timer < totalTime)
            {
                glitch.Intensity.value = GlitchIntensityByTimeSinceDeath.Evaluate(timer);
                timer += Time.deltaTime;

                if (timer >= DeathResetTime && !haveReset)
                {
                    haveReset = true;
                    DeathReset.Raise();
                }

                yield return null;
            }

            glitch.Intensity.value = 0;
            DeathGlitchEffectIsOn.Value = false;
        }
    }
}
