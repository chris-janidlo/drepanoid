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
        public TilesetFont Font;
        public SetTextOptions Options;
        [Min(0)] public int MaximumLinesOfHistoryToDisplay = 1;

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
                yield return Driver.Text.Delete(StartingPosition + Vector2Int.down * MaximumLinesOfHistoryToDisplay, new Vector2Int(maxLineLength, 1));

                currentLine = LogTextLines[lineCursor];

                var currentLineTextArguments = new SetTextArguments
                {
                    Text = currentLine.Text,
                    StartingPosition = StartingPosition + Vector2Int.down * MaximumLinesOfHistoryToDisplay,
                    Font = Font
                };

                yield return Driver.Text.SetText(currentLineTextArguments, Options);
                yield return new WaitForSeconds(currentLine.EndDelay);

                logHistory.Add(currentLine.Text);
                if (logHistory.Count > MaximumLinesOfHistoryToDisplay) logHistory.RemoveAt(0);

                yield return Driver.Text.Delete(StartingPosition, new Vector2Int(maxLineLength, -MaximumLinesOfHistoryToDisplay));

                var historyTextArguments = new SetTextArguments
                {
                    Text = string.Join("\n", logHistory),
                    StartingPosition = StartingPosition + Vector2Int.down * (MaximumLinesOfHistoryToDisplay - logHistory.Count),
                    Font = Font
                };

                var historyTextOptions = new SetTextOptions(null, Options.LoadAnimation, Options.Transformer); // ensure that entire block of text displays immediately
                StartCoroutine(Driver.Text.SetText(historyTextArguments, historyTextOptions));

                lineCursor++;
                lineCursor %= LogTextLines.Count;
            }
        }
    }
}