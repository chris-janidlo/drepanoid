using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

[CreateAssetMenu(menuName = "Character Animations for Level Transitions", fileName = "newCharacterAnimations.asset")]
public class CharacterAnimationsForLevelTransitions : ScriptableObject
{
    [Serializable]
    public struct AnimationFrame
    {
        public TileBase Tile;
        public Sprite Sprite;
    }

    public List<AnimationFrame> LevelLoadAnimation, LevelUnloadAnimation;
    public Vector2 FrameTimeRange;

    public IEnumerator AnimateSpriteRendererLoad (float showDelay, SpriteRenderer spriteRenderer)
    {
        yield return animateSpriteRendererInternal(showDelay, spriteRenderer, true);
    }

    public IEnumerator AnimateSpriteRendererUnload (float showDelay, SpriteRenderer spriteRenderer)
    {
        yield return animateSpriteRendererInternal(showDelay, spriteRenderer, false);
    }

    IEnumerator animateSpriteRendererInternal (float showDelay, SpriteRenderer spriteRenderer, bool loading)
    {
        Sprite finalSprite = loading ? spriteRenderer.sprite : null;

        if (loading) spriteRenderer.sprite = null;
        yield return new WaitForSeconds(showDelay);

        var frames = loading ? LevelLoadAnimation : LevelUnloadAnimation;
        foreach (var frame in frames)
        {
            spriteRenderer.sprite = frame.Sprite;
            yield return new WaitForSeconds(RandomExtra.Range(FrameTimeRange));
        }

        spriteRenderer.sprite = finalSprite;
    }
}
