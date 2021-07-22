using System;
using System.Linq;
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

            // copied from CharacterAnimation to reduce retrievals
            public TileBase[] Frames;
            public Vector2 FrameTimeRange;
        }
        
        class TilesetAnimationTracker
        {
            public Tilemap Tilemap;
            public TilePositionCollection TilePositionCollection;
            public float ShowDelayTimer;

            public List<TileAnimationTracker> TileData;
        }

        List<TilesetAnimationTracker> currentTileAnimations = new List<TilesetAnimationTracker>();

        void Update ()
        {
            for (int i = currentTileAnimations.Count - 1; i >= 0; i--)
            {
                TilesetAnimationTracker anim = currentTileAnimations[i];
                anim.ShowDelayTimer -= Time.deltaTime;
                if (anim.ShowDelayTimer <= 0)
                {
                    animateFrameTilemap(anim);
                    if (anim.TileData.Count == 0) currentTileAnimations.RemoveAt(i);
                }
            }
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
            List<TileAnimationTracker> tileData = new List<TileAnimationTracker>(tiles.Count);

            for (int i = 0; i < tiles.Count; i++)
            {
                tileData.Add(new TileAnimationTracker
                {
                    Position = tiles.Positions[i],
                    FinalTile = tiles.Tiles[i],
                    CurrentFrame = -1,
                    Frames = animation.Frames.Select(f => f.Tile).ToArray(),
                    FrameTimeRange = animation.FrameTimeRange
                });
            }

            TilesetAnimationTracker tilesetData = new TilesetAnimationTracker
            {
                Tilemap = tilemap,
                TilePositionCollection = new TilePositionCollection(tileData.Count),
                ShowDelayTimer = showDelay,
                TileData = tileData
            };
            currentTileAnimations.Add(tilesetData);

            yield return new WaitWhile(() => currentTileAnimations.Contains(tilesetData));
        }

        public void StopAnimations (Tilemap tilemap, List<Vector3Int> positions)
        {
            for (int i = currentTileAnimations.Count - 1; i >= 0; i--)
            {
                TilesetAnimationTracker anim = currentTileAnimations[i];
                if (anim.Tilemap != tilemap) continue;

                anim.TileData.RemoveAll(data => positions.Contains(data.Position));
                if (anim.TileData.Count == 0) currentTileAnimations.RemoveAt(i);
            }
        }

        public void StopAllAnimations (Tilemap tilemap)
        {
            for (int i = currentTileAnimations.Count - 1; i >= 0; i--)
            {
                TilesetAnimationTracker anim = currentTileAnimations[i];
                if (anim.Tilemap != tilemap) continue;

                anim.TileData.Clear();
                currentTileAnimations.RemoveAt(i);
            }
        }

        void animateFrameTilemap (TilesetAnimationTracker tilesetAnimation)
        {
            TilePositionCollection tilePositions = tilesetAnimation.TilePositionCollection;
            tilePositions.Clear();

            for (int i = tilesetAnimation.TileData.Count - 1; i >= 0; i--)
            {
                TileAnimationTracker data = tilesetAnimation.TileData[i];
                data.Timer -= Time.deltaTime;
                if (data.Timer > 0) continue;

                data.CurrentFrame++;
                data.Timer = RandomExtra.Range(data.FrameTimeRange);

                bool isFinished = data.CurrentFrame >= data.Frames.Length;
                TileBase currentTile = isFinished ? data.FinalTile : data.Frames[data.CurrentFrame];

                tilePositions.Add(data.Position, currentTile);

                if (isFinished) tilesetAnimation.TileData.RemoveAt(i);
            }

            tilesetAnimation.Tilemap.SetTiles(tilePositions.Positions, tilePositions.Tiles);
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
