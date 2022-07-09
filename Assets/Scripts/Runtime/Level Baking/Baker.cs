#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Drepanoid.LevelBaking
{
    [InitializeOnLoad]
    public static class Baker
    {
        static Baker ()
        {
            EditorApplication.playModeStateChanged += onExitingEditMode;
        }

        public static void BakeAll ()
        {
            doForAllBakedGoods(g => g.Bake());
        }

        public static void UnbakeAll ()
        {
            doForAllBakedGoods(g => g.Unbake());
        }

        public static void SaveCurrentScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        [MenuItem("Tools/Baker/Bake or Rebake all Baked Goods")]
        static void bakeCommand ()
        {
            BakeAll();
            SaveCurrentScene();
        }

        [MenuItem("Tools/Baker/Unbake all Baked Goods")]
        static void unbakeCommand ()
        {
            UnbakeAll();
            SaveCurrentScene();
        }

        static void onExitingEditMode (PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;

            bakeCommand();
        }

        static void doForAllBakedGoods (Action<IBakedGood> action)
        {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                var goods = root.GetComponentsInChildren<IBakedGood>(true);
                foreach (var good in goods)
                {
                    action(good);
                }
            }
        }
    }
}
#endif
