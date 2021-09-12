using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.SceneMgmt;
using UnityEngine.SceneManagement;

namespace Drepanoid
{
    public class TimedSceneLoader : MonoBehaviour
    {
        public float Time;
        public SceneField NextScene;

        IEnumerator Start ()
        {
            yield return new WaitForSeconds(Time);
            SceneManager.LoadScene(NextScene);
        }
    }
}
