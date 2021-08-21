using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [Serializable]
    public class DeleteTextOptions
    {
        [Header("Required Data")]
        public Vector2Int RegionStartPosition;
        public Vector2Int RegionExtents;

        [Header("Optional Data")]
        public SerializableNullable<int> CharactersPerSecondScroll;
        public CharacterAnimation Animation;

        public DeleteTextOptions ()
        {
            CharactersPerSecondScroll = new SerializableNullable<int>(null);
        }

        public DeleteTextOptions (Vector2Int regionStartPosition, Vector2Int regionExtents) : this()
        {
            RegionStartPosition = regionStartPosition;
            RegionExtents = regionExtents;
        }
    }
}