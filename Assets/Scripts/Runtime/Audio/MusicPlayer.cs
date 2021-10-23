using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace Drepanoid
{
    public class MusicPlayer : MonoBehaviour
    {
        private static MusicPlayer Instance;

        public MusicTrack Track;
        public AnimationCurve FadeOutCurve;
        public AudioSource Source;

        public FloatVariable MusicTrackPosition;

        IEnumerator fadeOutEnum;
        float lastRecordedTrackPosition;

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

        public void Update ()
        {
            if (fadeOutEnum == null) MusicTrackPosition.Value = Source.time;
        }

        public void OnMusicTrackPositionChanged (float value)
        {
            if (fadeOutEnum == null && value != lastRecordedTrackPosition) Source.time = value;
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
