using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drepanoid
{
    public class DeathGlitch : MonoBehaviour
    {
        public AnimationCurve GlitchIntensityByTimeSinceDeath;

        public PostProcessProfile PostProcessProfile;

        public void OnBallDied ()
        {
            StartCoroutine(deathRoutine());
        }

        IEnumerator deathRoutine ()
        {
            float timer = 0;
            float totalTime = GlitchIntensityByTimeSinceDeath.keys.Last().time;

            PostProcessProfile.TryGetSettings(out Glitch glitch);

            while (timer < totalTime)
            {
                glitch.Intensity.value = GlitchIntensityByTimeSinceDeath.Evaluate(timer);
                timer += Time.deltaTime;
                yield return null;
            }

            glitch.Intensity.value = 0;
        }
    }
}
