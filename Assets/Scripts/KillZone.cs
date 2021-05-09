using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;

public class KillZone : MonoBehaviour
{
    [Header("Baked collision stuff. Just here for validation/debugging's sake")]
    [SerializeField]
    private KillZone bakedCollisions;

    [SerializeField]
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
    void Bake ()
    {
        if (isABakedCollisionsObject) return;

        if (bakedCollisions) Unbake();

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
    void Unbake ()
    {
        if (!bakedCollisions || isABakedCollisionsObject) return;

        DestroyImmediate(bakedCollisions.gameObject);
        foreach (var component in PrefabUtility.GetRemovedComponents(gameObject))
        {
            PrefabUtility.RevertRemovedComponent(gameObject, component.assetComponent, InteractionMode.AutomatedAction);
        }

        StartCoroutine(saveNextFrame());
    }

    IEnumerator saveNextFrame ()
    {
        yield return null;
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
#endif // UNITY_EDITOR
}

