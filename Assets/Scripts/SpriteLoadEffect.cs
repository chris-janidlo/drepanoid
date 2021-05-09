using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterAnimationsForLevelTransitions Animation;
    public SpriteRenderer SpriteRenderer;

    void Start ()
    {
        StartCoroutine(Animation.AnimateSpriteRendererLoad(ShowDelay, SpriteRenderer));
    }

    public void OnLevelGoalReached ()
    {
        StartCoroutine(Animation.AnimateSpriteRendererUnload(ShowDelay, SpriteRenderer));
    }
}
