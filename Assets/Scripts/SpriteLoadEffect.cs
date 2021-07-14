using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    public class SpriteLoadEffect : MonoBehaviour
    {
        public float ShowDelay;
        public CharacterLoadAnimations Animation;
        public List<SpriteRenderer> SpriteRenderers;

        void Start ()
        {
            foreach (var spriteRenderer in SpriteRenderers)
            {
                StartCoroutine(Animation.AnimateSpriteRendererLoad(ShowDelay, spriteRenderer));
            }
        }

        public void OnLevelGoalReached ()
        {
            foreach (var spriteRenderer in SpriteRenderers)
            {
                StartCoroutine(Animation.AnimateSpriteRendererUnload(ShowDelay, spriteRenderer));
            }
        }
    }
}