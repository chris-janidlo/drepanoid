using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterAnimationsForLevelTransitions Animation;
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
