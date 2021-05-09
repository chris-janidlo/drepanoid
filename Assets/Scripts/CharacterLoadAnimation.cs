using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

[CreateAssetMenu(menuName = "Character Load Animation", fileName = "newCharacterAnimation.asset")]
public class CharacterLoadAnimation : ScriptableObject
{
    [Serializable]
    public struct AnimationFrame
    {
        public TileBase Tile;
        public Sprite Sprite;
    }

    public List<AnimationFrame> Frames;
    public Vector2 FrameTimeRange;

    public IEnumerator AnimateSpriteRenderer (float showDelay, SpriteRenderer spriteRenderer)
    {
        var initialSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = null;
        yield return new WaitForSeconds(showDelay);

        foreach (var frame in Frames)
        {
            spriteRenderer.sprite = frame.Sprite;
            yield return new WaitForSeconds(RandomExtra.Range(FrameTimeRange));
        }

        spriteRenderer.sprite = initialSprite;
    }
}
