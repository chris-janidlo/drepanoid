using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Drepanoid
{
    [RequireComponent(typeof(AudioBehaviour))]
    public class RetriggerFilter : MonoBehaviour
    {
        public List<float> PossibleSampleLengths;
        public bool Active;

        float[] currentSample => sampleHolders[currentSampleLength];

        int[] possibleSampleCounts;
        int currentSampleLength, sampleRecorderPointer, samplePlayerPointer;
        float[][] sampleHolders;
        Random random;

        void Start ()
        {
            possibleSampleCounts = PossibleSampleLengths
                .Select(l => Mathf.RoundToInt(l * AudioSettings.outputSampleRate))
                .ToArray();

            sampleHolders = new float[possibleSampleCounts.Last() + 1][];

            foreach (var count in possibleSampleCounts)
            {
                sampleHolders[count] = new float[count];
            }

            random = new Random();
        }

        void OnAudioFilterRead (float[] data, int channels)
        {
            if (!Active)
            {
                currentSampleLength = 0;
                return;
            }
            else if (currentSampleLength == 0)
            {
                currentSampleLength = possibleSampleCounts[random.Next(possibleSampleCounts.Length - 1)];
                sampleRecorderPointer = 0;
                samplePlayerPointer = 0;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (sampleRecorderPointer < currentSampleLength)
                {
                    currentSample[sampleRecorderPointer++] = data[i];
                }
                else
                {
                    // note that this will occasionally send samples to the wrong channel, since channel data is interleaved and currentSampleLength might be co-prime with 'channels'. should be fine though since it's a glitch effect
                    data[i] = currentSample[samplePlayerPointer++ % currentSampleLength];
                }
            }
        }
    }
}
