using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drepanoid
{
    [CreateAssetMenu(menuName = "Music Track", fileName = "newMusicTrack.asset")]
    public class MusicTrack : ScriptableObject
    {
        public AudioClip Clip;
        [Range(0, 1)]
        public float Volume = 1;
    }
}
