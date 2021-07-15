using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class SpriteLoadEffect : MonoBehaviour
    {
        public float ShowDelay;
        public CharacterAnimation LoadAnimation, UnloadAnimation;
        public List<SpriteRenderer> SpriteRenderers;

        void Start ()
        {
            foreach (var spriteRenderer in SpriteRenderers)
            {
                StartCoroutine(Driver.CharacterAnimations.AnimateSpriteRendererLoad(LoadAnimation, ShowDelay, spriteRenderer));
            }
        }

        public void OnLevelGoalReached ()
        {
            foreach (var spriteRenderer in SpriteRenderers)
            {
                StartCoroutine(Driver.CharacterAnimations.AnimateSpriteRendererUnload(UnloadAnimation, ShowDelay, spriteRenderer));
            }
        }
    }
}