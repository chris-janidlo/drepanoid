using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityAtoms.BaseAtoms;

[CreateAssetMenu(menuName = "Scene Transition Helper", fileName = "newSceneTransitionHelper.asset")]
public class SceneTransitionHelper : ScriptableObject
{
    [Tooltip("Should be derived from the length of the character animation (frame count * longest possible frame time) plus the time of the longest animation delay.")]
    public float LevelLoadAnimationTime, LevelUnloadAnimationTime;

    public VoidEvent LevelGoalReached;

    public IEnumerator UnloadCurrentLevelAndLoadNextRoutine (string nextLevelName)
    {
        LevelGoalReached.Raise();
        yield return new WaitForSeconds(LevelUnloadAnimationTime);

        var loadOperation = SceneManager.LoadSceneAsync(nextLevelName);
        yield return new WaitUntil(() => loadOperation.isDone);
    }
}
