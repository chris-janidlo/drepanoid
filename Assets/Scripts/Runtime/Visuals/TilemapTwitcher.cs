using System.Linq;
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
        public SceneTransitionHelper SceneTransitionHelper;

        IEnumerator Start ()
        {
            int maxPossibleTiles = Tilemap.cellBounds.size.x * Tilemap.cellBounds.size.y;
            List<Vector3Int> tilePositions = new List<Vector3Int>(maxPossibleTiles);
            List<Matrix4x4> originalMatrices = new List<Matrix4x4>(maxPossibleTiles);

            // for smearing the work to grab the tiles over multiple frames while the level loads and animates
            float timer = SceneTransitionHelper.LevelLoadAnimationTime;
            int expectedFramesToLoadLevel = Mathf.RoundToInt(timer * 60); // assumes a rough 60 fps, since unfortunately you can't retrieve Application.targetFrameRate's true default value (it's a special value of -1 by default)

            int positionsToCheckPerFrame = maxPossibleTiles / expectedFramesToLoadLevel;
            int positionsCheckedThisFrame = 0;

            foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
            {
                bool needToFinishNow = timer <= 0;

                if (!needToFinishNow) positionsCheckedThisFrame++;
                if (!Tilemap.HasTile(cellPosition)) continue;

                tilePositions.Add(cellPosition);
                originalMatrices.Add(Tilemap.GetTransformMatrix(cellPosition));

                if (!needToFinishNow && positionsCheckedThisFrame >= positionsToCheckPerFrame)
                {
                    positionsCheckedThisFrame = 0;
                    timer -= Time.deltaTime;
                    yield return null;
                }
            }

            tilePositions.TrimExcess();
            originalMatrices.TrimExcess();

            while (true)
            {
                yield return new WaitForSeconds(RandomExtra.Range(TimeBetweenTwitchesRange));

                for (int i = 0; i < tilePositions.Count; i++)
                {
                    if (!RandomExtra.Chance(ChanceToTwitchPerTile)) continue;

                    var matrix = originalMatrices[i];
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
                    Tilemap.SetTransformMatrix(tilePositions[i], matrix);
                }
            }
        }
    }
}