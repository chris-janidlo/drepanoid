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

        public void Play (AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
