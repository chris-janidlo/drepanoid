using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("PickupTextEffect")]
        public SetTextOptions PopupSetTextOptions;
        [FormerlySerializedAs("PickupTextEffectVisibilityTime")]
        public float PopupVisibilityTime;
        public CharacterAnimation PopupDeleteAnimation;

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

            StartCoroutine(deathRoutine());
        }

        public void OnLevelGoalReached ()
        {
            Animator.enabled = false;
            StartCoroutine(Driver.CharacterAnimations.AnimateSpriteRendererUnload(Animation, ShowAnimationDelay, SpriteRenderer));
        }

        IEnumerator deathRoutine ()
        {
            Collider.enabled = false;
            Animator.SetTrigger(AnimatorDeathTrigger);

            if (!alreadyCollected) CollectedCookies.Add(Cookie);

            yield return Driver.Text.SetText(PopupSetTextOptions);
            yield return new WaitForSeconds(PopupVisibilityTime);
            yield return Driver.Text.Delete(new DeleteTextOptions
            {
                RegionStartPosition = PopupSetTextOptions.StartingPosition,
                RegionExtents = new Vector2Int(PopupSetTextOptions.Text.Length, 1),
                Animation = PopupDeleteAnimation
            });

            Destroy(gameObject);
        }
    }
}