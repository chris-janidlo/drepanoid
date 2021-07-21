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
            public CharacterAnimation Animation;

            public TileBase CurrentTile => IsFinished ? FinalTile : Animation.Frames[CurrentFrame].Tile;
            public bool IsFinished => CurrentFrame >= Animation.Frames.Count;

            public void AdvanceFrame ()
            {
                CurrentFrame++;
                Timer = RandomExtra.Range(Animation.FrameTimeRange);
            }
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
                    Animation = animation
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

        void animateFrameTilemap (TilesetAnimationTracker tilesetAnimation)
        {
            TilePositionCollection tilePositions = tilesetAnimation.TilePositionCollection;
            tilePositions.Clear();

            foreach (var data in tilesetAnimation.TileData)
            {
                data.Timer -= Time.deltaTime;
                if (data.Timer > 0) continue;

                data.AdvanceFrame();
                tilePositions.Add(data.Position, data.CurrentTile);
            }

            tilesetAnimation.Tilemap.SetTiles(tilePositions.Positions, tilePositions.Tiles);
            tilesetAnimation.TileData.RemoveAll(d => d.IsFinished);
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
