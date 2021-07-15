using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class CookiePickup : MonoBehaviour
    {
        public Cookie Cookie;
        public Color NormalColor, AlreadyCollectedColor;
        public float FloatSpeed;
        public string AnimatorDeathTrigger;

        public CookieValueList CollectedCookies;
        public SpriteRenderer SpriteRenderer;
        public Collider2D Collider;
        public Animator Animator;

        public float ShowAnimationDelay;
        public CharacterAnimation Animation;

        bool alreadyCollected;
        Vector3 initialPosition;

        IEnumerator Start ()
        {
            alreadyCollected = CollectedCookies.Contains(Cookie);
            SpriteRenderer.color = alreadyCollected ? AlreadyCollectedColor : NormalColor;

            initialPosition = transform.position;

            Animator.enabled = false;
            yield return StartCoroutine(Driver.CharacterAnimations.AnimateSpriteRendererLoad(Animation, ShowAnimationDelay, SpriteRenderer));
            Animator.enabled = true;
        }

        void Update ()
        {
            var yOffset = Mathf.Sin(FloatSpeed * Time.time) / 8f;
            yOffset = Mathf.Round(yOffset * 8f) / 8f;
            transform.position = initialPosition + Vector3.up * yOffset;
        }

        void OnTriggerEnter2D (Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Ball>() == null) return;

            Animator.SetTrigger(AnimatorDeathTrigger);
            Collider.enabled = false;
        }

        public void OnLevelGoalReached ()
        {
            Animator.enabled = false;
            StartCoroutine(Driver.CharacterAnimations.AnimateSpriteRendererUnload(Animation, ShowAnimationDelay, SpriteRenderer));
        }

        public void FinalizeCollect ()
        {
            if (!alreadyCollected) CollectedCookies.Add(Cookie);
            Destroy(gameObject);
        }
    }
}