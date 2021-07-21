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
        public readonly Vector3Int[] Positions;
        public readonly TileBase[] Tiles;

        public int Count { get; private set; }

        public TilePositionCollection (int length)
        {
            Positions = new Vector3Int[length];
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
            Array.Clear(Positions, 0, Count);
            Array.Clear(Tiles, 0, Count);
            Count = 0;
        }
    }
}
