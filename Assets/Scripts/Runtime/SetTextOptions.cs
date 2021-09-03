using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [Serializable]
    public class SetTextOptions
    {
        [Header("Required Data")]
        [TextArea]
        public string Text;
        public Vector2Int StartingPosition;
        public TilesetFont Font;

        [Header("Optional Data")]
        public SerializableNullable<int> CharactersPerSecondScroll;
        public CharacterAnimation LoadAnimation;
    }
}