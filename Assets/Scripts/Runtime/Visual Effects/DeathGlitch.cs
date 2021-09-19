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
        public RetriggerFilter RetriggerFilter;

        public BoolVariable DeathGlitchEffectIsOn;
        public VoidEvent DeathReset;

        Glitch glitch;

        void Start ()
        {
            PostProcessProfile.TryGetSettings(out glitch);
            resetEffect();
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
            RetriggerFilter.Active = true; // TODO: when implementing music system, record the audio position at the start of the effect and restore back to that position at the end, so that the retrigger is more like a stall

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

            resetEffect();
        }

        void resetEffect ()
        {
            glitch.Intensity.value = 0;
            DeathGlitchEffectIsOn.Value = false;
            RetriggerFilter.Active = false;
        }
    }
}
