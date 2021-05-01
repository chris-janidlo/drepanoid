using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityAtoms.SceneMgmt;

public class Goal : MonoBehaviour
{
    public const float MIN_TIMESCALE = 0.01f;
    public SceneField TargetScene;
    public AnimationCurve TimescaleByTimeSinceCollision;

    public AnimationCurve LensDistortionIntensityByTimeSinceCollision, ChromaticAberrationByTimeSinceCollision;

    public PostProcessProfile PostProcessStack;
    public Collider2D Collider;

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() == null) return;

        Collider.enabled = false;
        StartCoroutine(goalHit());
    }

    IEnumerator goalHit ()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        LensDistortion lensLayer;
        ChromaticAberration chromaticAberrationLayer;
        PostProcessStack.TryGetSettings(out lensLayer);
        PostProcessStack.TryGetSettings(out chromaticAberrationLayer);

        float timer = 0;
        while (timer < TimescaleByTimeSinceCollision.keys.Last().time)
        {
            Time.timeScale = Mathf.Max(TimescaleByTimeSinceCollision.Evaluate(timer), MIN_TIMESCALE);
            lensLayer.intensity.value = LensDistortionIntensityByTimeSinceCollision.Evaluate(timer);
            chromaticAberrationLayer.intensity.value = ChromaticAberrationByTimeSinceCollision.Evaluate(timer);

            timer += Time.deltaTime;
            yield return null;
        }

        var loadOperation = SceneManager.LoadSceneAsync(TargetScene);
        yield return new WaitUntil(() => loadOperation.isDone);

        Time.timeScale = 1;
        lensLayer.intensity.value = LensDistortionIntensityByTimeSinceCollision.keys.First().value;
        chromaticAberrationLayer.intensity.value = ChromaticAberrationByTimeSinceCollision.keys.First().value;

        Destroy(gameObject);
    }
}
