using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using crass;

namespace Drepanoid
{
    public class ScanlineAudio : MonoBehaviour
    {
        [Range(0, 1)]
        public float WooWooVolume;

        public TransitionableFloat LoopFadeInTransition, LoopFadeOutTransition;

        public SoundEffect PickUpEffect, DropOffEffect; 

        public AudioSource WooWooSource;
        public SoundEffectPlayer SoundEffectPlayer;

        float fadeMultiplier;
        IEnumerator fadeRoutine;

        void Start ()
        {
            LoopFadeInTransition.AttachMonoBehaviour(this);
            LoopFadeOutTransition.AttachMonoBehaviour(this);
        }

        void Update ()
        {
            WooWooSource.volume = fadeMultiplier * WooWooVolume;
        }

        void OnTriggerEnter2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            StartCoroutine(fadeRoutine = fade(false));
            SoundEffectPlayer.Play(PickUpEffect);
        }

        void OnTriggerExit2D (Collider2D collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball == null) return;

            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            StartCoroutine(fadeRoutine = fade(true));
            SoundEffectPlayer.Play(DropOffEffect);
        }

        IEnumerator fade (bool fadeOut)
        {
            var transition = fadeOut ? LoopFadeOutTransition : LoopFadeInTransition;
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
