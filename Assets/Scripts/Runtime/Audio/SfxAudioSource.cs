using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    public class SfxAudioSource : MonoBehaviour
    {
        public bool Playing => audioSource.isPlaying;

        [SerializeField] private AudioSource audioSource;

        void Start ()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Play (AudioClip clip, float? volume = null)
        {
            if (volume.HasValue)
            {
                audioSource.PlayOneShot(clip, volume.Value);
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
