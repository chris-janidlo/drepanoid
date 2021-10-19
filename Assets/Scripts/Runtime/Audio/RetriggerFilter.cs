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
        public Vector2 SampleLengthRange;
        public bool Active;

        float[] currentSample => sampleHolders[currentSampleLength];

        Vector2Int sampleCountRange;
        int currentSampleLength, sampleRecorderPointer, samplePlayerPointer;
        float[][] sampleHolders;
        Random random;

        void Start ()
        {
            int sampleRate = AudioSettings.outputSampleRate;
            sampleCountRange = new Vector2Int
            (
                Mathf.RoundToInt(SampleLengthRange.x * sampleRate),
                Mathf.RoundToInt(SampleLengthRange.y * sampleRate)
            );

            sampleHolders = new float[sampleCountRange.y + 1][];

            for (int i = sampleCountRange.x; i <= sampleCountRange.y; i++)
            {
                sampleHolders[i] = new float[i];
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
                currentSampleLength = random.Next(sampleCountRange.x, sampleCountRange.y);
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
