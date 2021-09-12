using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Drepanoid
{
    // zero-garbage container for tracking repeated tile operations over multiple tiles
    // especially useful with Tilemap.SetTiles
    public class TilePositionCollection
    {
        // use this instead of default(Vector3Int) as the default value for the Positions array
        // otherwise, when calling Tilemap.SetTiles(Positions, Tiles), any (Vector3Int.zero, null) pairs after the Count-th Position/Tile pair would overwrite any legitimate instances of Vector3Int.zero in the Positions array
            // in other words, you wouldn't be able to set any tiles at (0, 0, 0) most of the time
        public static readonly Vector3Int SAFE_DEFAULT_POSITION = Vector3Int.forward;

        public readonly Vector3Int[] Positions;
        public readonly TileBase[] Tiles;

        public int Count { get; private set; }

        public TilePositionCollection (int length)
        {
            Positions = new Vector3Int[length];
            for (int i = 0; i < length; i++)
            {
                Positions[i] = SAFE_DEFAULT_POSITION;
            }

            Tiles = new TileBase[length];
            Count = 0;
        }

        public void Add (Vector3Int position, TileBase tile)
        {
            Positions[Count] = position;
            Tiles[Count] = tile;
            Count++;
        }

        public void Clear ()
        {
            for (int i = 0; i < Count; i++)
            {
                Positions[i] = SAFE_DEFAULT_POSITION;
            }

            Array.Clear(Tiles, 0, Count);
            Count = 0;
        }
    }
}
