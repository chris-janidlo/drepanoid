using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

namespace Drepanoid
{
    public class TilemapTwitcher : MonoBehaviour
    {
        public float TileDisplacementRadius;
        public bool RountTilesToNearestPixel;
        public Vector2 TimeBetweenTwitchesRange;
        [Range(0, 1)]
        public float ChanceToTwitchPerTile;

        public Tilemap Tilemap;

        IEnumerator Start ()
        {
            while (true)
            {
                yield return new WaitForSeconds(RandomExtra.Range(TimeBetweenTwitchesRange));
                moveTiles();
            }
        }

        void moveTiles ()
        {
            foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
            {
                if (Tilemap.GetTile(cellPosition) == null) continue;
                if (!RandomExtra.Chance(ChanceToTwitchPerTile)) continue;

                var matrix = Tilemap.GetTransformMatrix(cellPosition);
                var position = Random.insideUnitCircle * TileDisplacementRadius;

                if (RountTilesToNearestPixel)
                {
                    position = new Vector2
                    (
                        Mathf.Round(position.x * 8) / 8,
                        Mathf.Round(position.y * 8) / 8
                    );
                }

                matrix[0, 3] = position.x;
                matrix[1, 3] = position.y;
                Tilemap.SetTransformMatrix(cellPosition, matrix);
            }
        }
    }
}