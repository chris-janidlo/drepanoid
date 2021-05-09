using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityAtoms.SceneMgmt;

public class Goal : MonoBehaviour
{
    public const float MIN_TIMESCALE = 0.01f;
    public SceneField TargetScene;
    public float LevelUnloadAnimationTime;

    public Collider2D Collider;

    public VoidEvent LevelGoalReached;

    void OnTriggerEnter2D (Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        Collider.enabled = false;
        ball.DespawnVictoriously();

        StartCoroutine(unloadLevel());
    }

    IEnumerator unloadLevel ()
    {
        transform.parent = null;

        LevelGoalReached.Raise();
        yield return new WaitForSeconds(LevelUnloadAnimationTime);

        DontDestroyOnLoad(gameObject);
        var loadOperation = SceneManager.LoadSceneAsync(TargetScene);
        yield return new WaitUntil(() => loadOperation.isDone);

        Destroy(gameObject);
    }
}
