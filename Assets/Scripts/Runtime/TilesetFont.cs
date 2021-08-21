using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Tileset Font", fileName = "newTilesetFont.asset")]
    public class TilesetFont : ScriptableObject
    {
        public const int PRINTABLE_CHARACTER_CODE_RANGE_START = 32;

        public List<Tile> PrintableAsciiMap;
        [Min(1)]
        public int SpacesPerTab;

        public bool CanPrint (int code)
        {
            bool result = code == '\n' || code == '\t' || PrintableAsciiMap.ElementAtOrDefault(code - PRINTABLE_CHARACTER_CODE_RANGE_START) != null;
            if (!result) Debug.Log(code);
            return result;
        }

        public Tile GetPrintableAsciiCharacter (int code)
        {
            return PrintableAsciiMap[code - PRINTABLE_CHARACTER_CODE_RANGE_START];
        }
    }
}