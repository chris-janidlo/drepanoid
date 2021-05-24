using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.SceneMgmt;

public class Goal : MonoBehaviour
{
    public const float MIN_TIMESCALE = 0.01f;
    public SceneField TargetScene;

    public Collider2D Collider;

    public SceneTransitionHelper SceneTransitionHelper;

    void OnTriggerEnter2D (Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) return;

        Collider.enabled = false;
        ball.DespawnVictoriously();

        StartCoroutine(SceneTransitionHelper.UnloadCurrentLevelAndLoadNextRoutine(TargetScene));
    }
}
