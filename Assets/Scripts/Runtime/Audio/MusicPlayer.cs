using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    public class MusicPlayer : MonoBehaviour
    {
        private static MusicPlayer Instance;

        public MusicTrack Track;
        public AnimationCurve FadeOutCurve;
        public AudioSource Source;

        IEnumerator fadeOutEnum;

        public void Start ()
        {
            if (Track == null || Instance != null && Track == Instance.Track)
            {
                Destroy(gameObject);
                return;
            }

            if (Instance != null) Instance.FadeOut();

            Instance = this;

            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            Source.clip = Track.Clip;
            Source.volume *= Track.Volume;
            Source.loop = true;
            Source.Play();
        }

        private void FadeOut ()
        {
            if (fadeOutEnum == null) StartCoroutine(fadeOutEnum = fadeOutRoutine());
        }

        IEnumerator fadeOutRoutine ()
        {
            float maxVolume = Source.volume;
            float timer = 0, time = FadeOutCurve.keys.Last().time;

            while (timer <= time)
            {
                Source.volume = maxVolume * FadeOutCurve.Evaluate(timer);
                timer += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
