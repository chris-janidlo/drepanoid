using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;
using crass;

namespace Drepanoid
{
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

        class TileAnimationTracker
        {
            public Vector3Int Position;
            public TileBase FinalTile;
            public int CurrentFrame;
            public float Timer;
            public List<AnimationFrame> Frames;

            public TileBase CurrentTile => IsFinished ? FinalTile : Frames[CurrentFrame].Tile;
            public bool IsFinished => CurrentFrame >= Frames.Count;
        }

        public IEnumerator AnimateSpriteRendererLoad (float showDelay, SpriteRenderer spriteRenderer)
        {
            yield return animateSpriteRendererInternal(showDelay, spriteRenderer, true);
        }

        public IEnumerator AnimateSpriteRendererUnload (float showDelay, SpriteRenderer spriteRenderer)
        {
            yield return animateSpriteRendererInternal(showDelay, spriteRenderer, false);
        }

        public IEnumerator AnimateTileset (float showDelay, Tilemap tilemap, TilePositionCollection tiles, bool loading)
        {
            List<TileAnimationTracker> animationData = new List<TileAnimationTracker>(tiles.Count);

            for (int i = 0; i < tiles.Count; i++)
            {
                animationData.Add(new TileAnimationTracker
                {
                    Position = tiles.Positions[i],
                    FinalTile = tiles.Tiles[i],
                    CurrentFrame = -1,
                    Frames = loading ? LoadAnimation : UnloadAnimation
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

                    data.Timer = RandomExtra.Range(FrameTimeRange);
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
}