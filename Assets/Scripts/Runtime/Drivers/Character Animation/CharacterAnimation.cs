using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Serialization;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Character Animation", fileName = "newCharacterAnimation.asset")]
    public class CharacterAnimation : ScriptableObject
    {
        [Serializable]
        public struct AnimationFrame
        {
            public TileBase Tile;
            public Sprite Sprite;
        }

        public List<AnimationFrame> Frames;
        public Vector2 FrameTimeRange;
    }
}