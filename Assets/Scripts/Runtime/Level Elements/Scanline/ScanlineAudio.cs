using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using crass;

namespace Drepanoid
{
    public class ScanlineAudio : MonoBehaviour
    {
        public AnimationCurve BallVerticalSpeedToFilterCutoffFrequency, FilterCutoffFrequencyToVolume;
        public float FilterBandWidth;
        public TransitionableFloat FadeInTransition, FadeOutTransition;

        public AudioSource Source;
        public AudioLowPassFilter LowPassFilter;
        public AudioHighPassFilter HighPassFilter;

        public float filterMidpoint, fadeMultiplier;

        void Start ()
        {
            FadeInTransition.AttachMonoBehaviour(this);
            FadeOutTransition.AttachMonoBehaviour(this);
        }

        void Update ()
        {
            Source.volume = fadeMultiplier * FilterCutoffFrequencyToVolume.Evaluate(filterMidpoint);
        }

        void OnTriggerStay2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            if (fadeMultiplier == 0) StartCoroutine(fade(false));

            filterMidpoint =
                BallVerticalSpeedToFilterCutoffFrequency.Evaluate(Mathf.Abs(ball.Velocity.y));
            LowPassFilter.cutoffFrequency = filterMidpoint + FilterBandWidth / 2;
            HighPassFilter.cutoffFrequency = filterMidpoint - FilterBandWidth / 2;
        }

        void OnTriggerExit2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            StartCoroutine(fade(true));
        }

        IEnumerator fade (bool fadeOut)
        {
            var transition = fadeOut ? FadeOutTransition : FadeInTransition;
            float start = fadeOut ? 1 : 0, end = fadeOut ? 0 : 1;
            transition.FlashFromTo(start, end);

            while (transition.Transitioning)
            {
                fadeMultiplier = transition.Value;
                yield return null;
            }

            fadeMultiplier = end;
        }
    }
}
