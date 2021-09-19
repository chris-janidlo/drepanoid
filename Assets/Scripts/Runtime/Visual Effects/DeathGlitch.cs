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
        public AnimationCurve GlitchIntensityByTimeSinceDeath;

        public PostProcessProfile PostProcessProfile;
        public BoolVariable DeathGlitchEffectIsOn;

        public void OnBallDied ()
        {
            StartCoroutine(deathRoutine());
        }

        IEnumerator deathRoutine ()
        {
            float timer = 0;
            float totalTime = GlitchIntensityByTimeSinceDeath.keys.Last().time;

            PostProcessProfile.TryGetSettings(out Glitch glitch);

            DeathGlitchEffectIsOn.Value = true;

            while (timer < totalTime)
            {
                glitch.Intensity.value = GlitchIntensityByTimeSinceDeath.Evaluate(timer);
                timer += Time.deltaTime;
                yield return null;
            }

            glitch.Intensity.value = 0;
            DeathGlitchEffectIsOn.Value = false;
        }
    }
}
