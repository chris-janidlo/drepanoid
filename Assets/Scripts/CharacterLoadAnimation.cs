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

    public IEnumerator DoAnimation (float showDelay, Action<AnimationFrame> applyFrame, TileBase resultTile = null, Sprite resultSprite = null)
    {
        applyFrame(new AnimationFrame { Tile = null, Sprite = null });
        yield return new WaitForSeconds(showDelay);

        foreach (var frame in Frames)
        {
            applyFrame(frame);
            yield return new WaitForSeconds(RandomExtra.Range(FrameTimeRange));
        }

        applyFrame(new AnimationFrame { Tile = resultTile, Sprite = resultSprite });
    }
}
