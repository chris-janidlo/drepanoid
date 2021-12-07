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

        public void Play (SoundEffect soundEffect)
        {
            audioSource.PlayOneShot(soundEffect.Clip, soundEffect.Volume);
        }
    }
}
