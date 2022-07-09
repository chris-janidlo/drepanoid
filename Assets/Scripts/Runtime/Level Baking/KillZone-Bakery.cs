#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Drepanoid.LevelBaking;

namespace Drepanoid
{
    public partial class KillZone : MonoBehaviour, IBakedGood
    {
        public void Bake()
        {
            if (_isABakedArtifact) return;

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
