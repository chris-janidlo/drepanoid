using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEditor;
using Drepanoid.LevelBaking;

namespace Drepanoid
{
    public partial class KillZone : MonoBehaviour, IBakedGood
    {
        public void Bake()
        {
            if (_isABakedArtifact || !needsBaking()) return;

            if (_bakedArtifact != null) Unbake();

            _bakedArtifact = Instantiate(this, transform.parent);
            _bakedArtifact._isABakedArtifact = true;

            DestroyImmediate(_bakedArtifact.GetComponent<TilemapLoadEffect>());
            DestroyImmediate(_bakedArtifact.GetComponent<TilemapTwitcher>());
            _bakedArtifact.GetComponent<Tilemap>().color = Color.clear;

            string snake_case = name.ToLower().Replace(' ', '_');
            _bakedArtifact.name = $"_{snake_case}__baked_collisions";

            DestroyImmediate(GetComponent<Collider2D>());

            // ensures that _bakedArtifact gets saved properly
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }

        public void Unbake()
        {
            if (_bakedArtifact == null || _isABakedArtifact) return;

            DestroyImmediate(_bakedArtifact.gameObject);
            _bakedArtifact = null;
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);

            foreach (var component in PrefabUtility.GetRemovedComponents(gameObject))
            {
                PrefabUtility.RevertRemovedComponent(gameObject, component.assetComponent, InteractionMode.AutomatedAction);
            }
        }

        // returns true if we haven't baked before, or if there are differences between the source tilemap and the current baked tilemap
        bool needsBaking ()
        {
            if (_bakedArtifact == null) return true;

            HashSet<(Vector3Int Position, Sprite Sprite, Color Color)> getTilesForComparison (KillZone killZone)
            {
                var tilemap = killZone.GetComponent<Tilemap>();
                var allTileData = new HashSet<(Vector3Int Position, Sprite Sprite, Color Color)>();

                foreach (var position in tilemap.cellBounds.allPositionsWithin)
                {
                    TileBase tileBase = tilemap.GetTile(position);
                    if (tileBase is Tile tile)
                    {
                        allTileData.Add((
                            Position: position,
                            Sprite: tile.sprite,
                            Color: tile.color
                        ));
                    }
                    else if (tileBase != null)
                    {
                        throw new BakeException($"unexpected non-Tile {tileBase.name} at position {position} in {killZone.name}");
                    }
                }

                return allTileData;
            }

            var sourceTiles = getTilesForComparison(this);
            var bakedTiles = getTilesForComparison(_bakedArtifact);

            return !sourceTiles.SetEquals(bakedTiles);
        }
    }
}
#endif

namespace Drepanoid
{
    public partial class KillZone : MonoBehaviour
    {
        /// <summary>
        /// Used for level baking. Can otherwise be ignored.
        /// </summary>
        [SerializeField, HideInInspector]
        private KillZone _bakedArtifact;

        /// <summary>
        /// Used for level baking. Can otherwise be ignored.
        /// </summary>
        [SerializeField, HideInInspector]
        private bool _isABakedArtifact;
    }
}
