using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Drepanoid
{
    [Serializable]
    public struct SetTextArguments
    {
        public string Text;
        public Vector2Int StartingPosition;
        public TilesetFont Font;

        public SetTextArguments (string text, TilesetFont font, Vector2Int startingPosition)
        {
            Text = text;
            Font = font;
            StartingPosition = startingPosition;
        }

        // returns the equivalent arguments needed to delete this text
        public DeleteTextArguments DeleteArguments ()
        {
            return new DeleteTextArguments(StartingPosition, new Vector2Int(Text.Length, 1));
        }
    }

    [Serializable]
    public struct SetTextOptions
    {
        [FormerlySerializedAs("CharactersPerSecondScroll")]
        [SerializeField] SerializableNullable<int> m_CharactersPerSecondScroll;
        public int? CharactersPerSecondScroll
        {
            get { initScroll(); return m_CharactersPerSecondScroll.ToNullable; }
            set { initScroll(); m_CharactersPerSecondScroll.ToNullable = value; }
        }

        public CharacterAnimation LoadAnimation;
        public TextTransformer Transformer;

        public SetTextOptions (int? charactersPerSecondScroll, CharacterAnimation loadAnimation, TextTransformer transformer)
        {
            m_CharactersPerSecondScroll = new SerializableNullable<int>(charactersPerSecondScroll);
            LoadAnimation = loadAnimation;
            Transformer = transformer;
        }

        void initScroll ()
        {
            if (m_CharactersPerSecondScroll != null) return;
            m_CharactersPerSecondScroll = new SerializableNullable<int>();
        }
    }
}