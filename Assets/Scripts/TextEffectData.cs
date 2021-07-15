using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [Serializable]
    public class TextEffectData : IEquatable<TextEffectData>
    {
        [Header("Required Data")]
        public string Text;
        public Vector2Int StartingPosition;
        public TilesetFont Font;

        [Header("Optional Data")]
        public SerializableNullable<int> CharactersPerSecondScroll;
        public CharacterAnimation LoadAnimation;

        public bool Equals (TextEffectData other)
        {
            return base.Equals(other);
        }
    }
}