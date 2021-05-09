using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterLoadAnimation Animation;
    public SpriteRenderer SpriteRenderer;

    void Start ()
    {
        StartCoroutine(Animation.AnimateSpriteRenderer(ShowDelay, SpriteRenderer));
    }
}
