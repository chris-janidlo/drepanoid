using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class TilemapLoadEffect : MonoBehaviour
    {
        public float ShowDelay;
        public CharacterAnimation LoadAnimation, UnloadAnimation;
        public TileBase EmptyTile;

        public Tilemap Tilemap;
        public TilemapCollider2D TilemapCollider;

        void Start ()
        {
            StartCoroutine(playAnimation(true));
        }

        public void OnLevelGoalReached ()
        {
            StartCoroutine(playAnimation(false));
        }

        IEnumerator playAnimation (bool loading)
        {
            if (TilemapCollider != null) TilemapCollider.enabled = false;

            BoundsInt bounds = Tilemap.cellBounds;
            int maximumTileCount = (bounds.xMax - bounds.xMin) * (bounds.yMax - bounds.yMin);

            TilePositionCollection tiles = new TilePositionCollection(maximumTileCount);

            foreach (var cellPosition in bounds.allPositionsWithin)
            {
                var tile = Tilemap.GetTile(cellPosition);
                if (tile == null) continue;

                tiles.Add(cellPosition, loading ? tile : EmptyTile);

                if (loading) Tilemap.SetTile(cellPosition, EmptyTile);
            }

            yield return Driver.CharacterAnimations.AnimateTileset(loading ? LoadAnimation : UnloadAnimation, ShowDelay, Tilemap, tiles);

            if (TilemapCollider != null && loading) TilemapCollider.enabled = true;
        }
    }
}