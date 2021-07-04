using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;
using crass;

[CreateAssetMenu(menuName = "Character Animations for Level Transitions", fileName = "newCharacterAnimations.asset")]
public class CharacterLoadAnimations : ScriptableObject
{
    [Serializable]
    public struct AnimationFrame
    {
        public TileBase Tile;
        public Sprite Sprite;
    }

    [FormerlySerializedAs("LevelLoadAnimation")]
    public List<AnimationFrame> LoadAnimation;
    [FormerlySerializedAs("LevelUnloadAnimation")]
    public List<AnimationFrame> UnloadAnimation;
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

        var frames = loading ? LoadAnimation : UnloadAnimation;
        foreach (var frame in frames)
        {
            spriteRenderer.sprite = frame.Sprite;
            yield return new WaitForSeconds(RandomExtra.Range(FrameTimeRange));
        }

        spriteRenderer.sprite = finalSprite;
    }
}
