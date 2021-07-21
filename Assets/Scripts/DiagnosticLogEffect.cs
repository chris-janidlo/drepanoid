using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drepanoid.Drivers;

namespace Drepanoid
{
    public class DiagnosticLogEffect : MonoBehaviour
    {
        [Serializable]
        public class LogLine
        {
            public string Text;
            public float EndDelay;
        }

        [Header("Data")]
        public List<LogLine> LogTextLines;
        public Vector2Int StartingPosition;
        [Min(1)] public int CharactersPerSecondScroll = 1;
        [Min(0)] public int MaximumLinesOfHistoryToDisplay = 1;
        public TilesetFont Font;
        public CharacterAnimation LoadAnimation;

        public void OnLevelGoalReached ()
        {
            StopAllCoroutines();
        }

        IEnumerator Start ()
        {
            int lineCursor = 0;
            LogLine currentLine;
            List<string> logHistory = new List<string>(MaximumLinesOfHistoryToDisplay + 1);

            int maxLineLength = LogTextLines.Max(l => l.Text.Length);

            while (true)
            {
                Driver.Text.Delete(StartingPosition + Vector2Int.down * MaximumLinesOfHistoryToDisplay, new Vector2Int(maxLineLength, 1));

                currentLine = LogTextLines[lineCursor];

                var currentLineTextEffectData = new TextEffectData
                {
                    Text = currentLine.Text,
                    StartingPosition = StartingPosition + Vector2Int.down * MaximumLinesOfHistoryToDisplay,
                    Font = Font,
                    CharactersPerSecondScroll = new SerializableNullable<int>(CharactersPerSecondScroll),
                    LoadAnimation = LoadAnimation
                };

                yield return Driver.Text.SetText(currentLineTextEffectData);
                yield return new WaitForSeconds(currentLine.EndDelay);

                logHistory.Add(currentLine.Text);
                if (logHistory.Count > MaximumLinesOfHistoryToDisplay) logHistory.RemoveAt(0);

                Driver.Text.Delete(StartingPosition, new Vector2Int(maxLineLength, -MaximumLinesOfHistoryToDisplay));

                var historyTextEffectData = new TextEffectData
                {
                    Text = string.Join("\n", logHistory),
                    StartingPosition = StartingPosition + Vector2Int.down * (MaximumLinesOfHistoryToDisplay - logHistory.Count),
                    Font = Font,
                    CharactersPerSecondScroll = new SerializableNullable<int>(null), // ensures that entire block of text displays immediately
                    LoadAnimation = LoadAnimation
                };

                StartCoroutine(Driver.Text.SetText(historyTextEffectData));

                lineCursor++;
                lineCursor %= LogTextLines.Count;
            }
        }
    }
}