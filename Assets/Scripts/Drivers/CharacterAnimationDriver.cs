using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

namespace Drepanoid.Drivers
{
    public class CharacterAnimationDriver : MonoBehaviour
    {
        class TileAnimationTracker
        {
            public Vector3Int Position;
            public TileBase FinalTile;
            public int CurrentFrame;
            public float Timer;
            public List<CharacterAnimation.AnimationFrame> Frames;

            public TileBase CurrentTile => IsFinished ? FinalTile : Frames[CurrentFrame].Tile;
            public bool IsFinished => CurrentFrame >= Frames.Count;
        }

        public IEnumerator AnimateSpriteRendererLoad (CharacterAnimation animation, float showDelay, SpriteRenderer spriteRenderer)
        {
            yield return animateSpriteRendererInternal(animation, showDelay, spriteRenderer, true);
        }

        public IEnumerator AnimateSpriteRendererUnload (CharacterAnimation animation, float showDelay, SpriteRenderer spriteRenderer)
        {
            yield return animateSpriteRendererInternal(animation, showDelay, spriteRenderer, false);
        }

        public IEnumerator AnimateTileset (CharacterAnimation animation, float showDelay, Tilemap tilemap, TilePositionCollection tiles)
        {
            List<TileAnimationTracker> animationData = new List<TileAnimationTracker>(tiles.Count);

            for (int i = 0; i < tiles.Count; i++)
            {
                animationData.Add(new TileAnimationTracker
                {
                    Position = tiles.Positions[i],
                    FinalTile = tiles.Tiles[i],
                    CurrentFrame = -1,
                    Frames = animation.Frames
                });
            }

            yield return new WaitForSeconds(showDelay);

            Vector3Int[] positionArray = new Vector3Int[animationData.Count];
            TileBase[] tileArray = new TileBase[animationData.Count];
            int cursor = 0;

            while (animationData.Count > 0)
            {
                foreach (var data in animationData)
                {
                    data.Timer -= Time.deltaTime;
                    if (data.Timer > 0) continue;

                    data.Timer = RandomExtra.Range(animation.FrameTimeRange);
                    data.CurrentFrame++;

                    positionArray[cursor] = data.Position;
                    tileArray[cursor] = data.CurrentTile;
                    cursor++;
                }

                tilemap.SetTiles(positionArray, tileArray);
                animationData.RemoveAll(anim => anim.IsFinished);

                yield return null;

                Array.Clear(positionArray, 0, cursor);
                Array.Clear(tileArray, 0, cursor);
                cursor = 0;
            }
        }

        IEnumerator animateSpriteRendererInternal (CharacterAnimation animation, float showDelay, SpriteRenderer spriteRenderer, bool loading)
        {
            Sprite finalSprite = loading ? spriteRenderer.sprite : null;

            if (loading) spriteRenderer.sprite = null;
            yield return new WaitForSeconds(showDelay);

            var frames = animation.Frames;
            foreach (var frame in frames)
            {
                spriteRenderer.sprite = frame.Sprite;
                yield return new WaitForSeconds(RandomExtra.Range(animation.FrameTimeRange));
            }

            spriteRenderer.sprite = finalSprite;
        }
    }
}
