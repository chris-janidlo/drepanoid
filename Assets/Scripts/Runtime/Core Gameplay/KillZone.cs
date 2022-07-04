using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityAtoms.BaseAtoms;

namespace Drepanoid
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private KillZone bakedCollisions;
        [SerializeField, HideInInspector]
        private bool isABakedCollisionsObject;

        void OnTriggerEnter2D (Collider2D collision)
        {
            Ball ball = collision.GetComponent<Ball>();

            if (ball != null)
            {
                ball.Kill();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Bake or Rebake")]
        public void Bake ()
        {
            if (isABakedCollisionsObject) return;

            if (bakedCollisions != null) unbake(save: false);

            bakedCollisions = Instantiate(this, transform.parent);
            bakedCollisions.isABakedCollisionsObject = true;

            DestroyImmediate(bakedCollisions.GetComponent<TilemapLoadEffect>());
            DestroyImmediate(bakedCollisions.GetComponent<TilemapTwitcher>());
            bakedCollisions.GetComponent<Tilemap>().color = Color.clear;
            bakedCollisions.name = $"_{name} (baked collisions)";

            DestroyImmediate(GetComponent<Collider2D>());

            StartCoroutine(saveNextFrame());
        }

        [ContextMenu("Unbake")]
        public void Unbake ()
        {
            unbake(save: true);
        }

        // this has to be its own method because ContextMenu methods can only have very specific argument signatures
        void unbake (bool save)
        {
            if (bakedCollisions == null || isABakedCollisionsObject) return;

            DestroyImmediate(bakedCollisions.gameObject);
            foreach (var component in PrefabUtility.GetRemovedComponents(gameObject))
            {
                PrefabUtility.RevertRemovedComponent(gameObject, component.assetComponent, InteractionMode.AutomatedAction);
            }

            if (save) StartCoroutine(saveNextFrame());
        }

        IEnumerator saveNextFrame()
        {
            yield return null;
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
#endif // UNITY_EDITOR
    }
}
