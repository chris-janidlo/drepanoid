using UnityEngine;
using UnityAtoms.BaseAtoms;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

namespace Drepanoid
{
#if UNITY_EDITOR
    [ExecuteAlways]
    [InitializeOnLoad]
#endif
    public class BallPositionTracer : MonoBehaviour
    {
        public bool DrawGizmos;

        public float RecordsPerSecond;
        public int RecordCount;

        public Vector2Variable BallPosition;

        public string GizmoIconName;

#if UNITY_EDITOR
        Vector2[] recordBuffer;
        float recordTimer;
        int recordCounter;

        static BallPositionTracer ()
        {
            EditorApplication.playModeStateChanged += s =>
            {
                var worker = FindObjectOfType<BallPositionTracer>();
                if (worker != null) worker.onPlayModeStateChanged(s);
            };
        }

        void Start ()
        {
            if (!Application.IsPlaying(gameObject)) return;

            recordBuffer = Enumerable.Repeat(Vector2.one * Mathf.Infinity, RecordCount).ToArray();
            recordCounter = 0;
        }

        void Update ()
        {
            if (!Application.IsPlaying(gameObject)) return;

            recordTimer += Time.deltaTime;

            if (recordTimer >= 1f / RecordsPerSecond)
            {
                recordTimer = 0;

                recordBuffer[recordCounter] = BallPosition.Value;

                recordCounter++;
                recordCounter %= RecordCount;
            }
        }

        void OnDrawGizmos ()
        {
            if (recordBuffer == null || !DrawGizmos) return;

            foreach (var record in recordBuffer)
            {
                Gizmos.DrawIcon(record, GizmoIconName);
            }
        }

        void onPlayModeStateChanged (PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    persistBuffer(recordBuffer);
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    recordBuffer = loadBuffer(RecordCount);
                    break;
            }
        }

        const string PREF_KEY_PREFIX = "__editor_ball_pos_buffer";
        (string, string) prefsKeys (int i) => ($"{PREF_KEY_PREFIX}[{i}].x", $"{PREF_KEY_PREFIX}[{i}].y");

        void persistBuffer (Vector2[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                var (xKey, yKey) = prefsKeys(i);

                PlayerPrefs.SetFloat(xKey, buffer[i].x);
                PlayerPrefs.SetFloat(yKey, buffer[i].y);
            }

            PlayerPrefs.Save();
        }

        Vector2[] loadBuffer (int length)
        {
            Vector2[] buffer = new Vector2[length];

            for (int i = 0; i < buffer.Length; i++)
            {
                var (xKey, yKey) = prefsKeys(i);

                if (!PlayerPrefs.HasKey(xKey) || !PlayerPrefs.HasKey(yKey))
                {
                    return null;
                }

                buffer[i] = new Vector2
                {
                    x = PlayerPrefs.GetFloat(xKey),
                    y = PlayerPrefs.GetFloat(yKey),
                };

                PlayerPrefs.DeleteKey(xKey);
                PlayerPrefs.DeleteKey(yKey);
            }

            PlayerPrefs.Save();
            return buffer;
        }
#else
        void Start ()
        {
            Destroy(gameObject);
        }
#endif // UNITY_EDITOR
    }
}
