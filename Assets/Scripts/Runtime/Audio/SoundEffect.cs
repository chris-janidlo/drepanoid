using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Sound Effect", fileName = "newSoundAffect.asset")]
    public class SoundEffect : ScriptableObject
    {
        public AudioClip Clip;
        [Range(0, 1)]
        public float Volume = 1;
    }
}
