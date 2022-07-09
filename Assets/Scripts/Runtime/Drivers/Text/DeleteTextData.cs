using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Drepanoid
{
    [Serializable]
    public struct DeleteTextArguments
    {
        public Vector2Int RegionStartPosition;
        public Vector2Int RegionExtents;

        public DeleteTextArguments (Vector2Int regionStartPosition, Vector2Int regionExtents)
        {
            RegionStartPosition = regionStartPosition;
            RegionExtents = regionExtents;
        }
    }

    [Serializable]
    public struct DeleteTextOptions
    {
        [FormerlySerializedAs("CharactersPerSecondScroll")]
        SerializableNullable<int> m_CharactersPerSecondScroll;
        public int? CharactersPerSecondScroll
        {
            get => m_CharactersPerSecondScroll.ToNullable;
            set => m_CharactersPerSecondScroll.ToNullable = value;
        }

        public CharacterAnimation Animation;
    }
}