using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Drepanoid.Drivers
{
    public class TextDriver : MonoBehaviour
    {
        public Tilemap MainTextTilemap, TextShadowTilemap;

        class SetTextAnimationTracker
        {
            public string CleanedText;
            public float Timer;
            public SetTextOptions Options;
            public Vector3Int PositionCursor;
            public int TextCursor;
            public TilePositionCollection TilePositionCollection;
        }

        bool endingLevel;
        List<SetTextAnimationTracker> currentAnimations = new List<SetTextAnimationTracker>();

        void Update ()
        {
            if (endingLevel) return;

            for (int i = currentAnimations.Count - 1; i >= 0; i--)
            {
                SetTextAnimationTracker tracker = currentAnimations[i];
                tracker.Timer -= Time.deltaTime;
                if (tracker.Timer <= 0) animateFrame(tracker);
                if (tracker.TextCursor >= tracker.CleanedText.Length) currentAnimations.RemoveAt(i);
            }
        }

        public void OnLevelGoalReached ()
        {
            Driver.CharacterAnimations.StopAllAnimations(MainTextTilemap);
            Driver.CharacterAnimations.StopAllAnimations(TextShadowTilemap);
            currentAnimations.Clear();
            endingLevel = true;
        }

        public IEnumerator SetText (SetTextOptions options)
        {
            if (endingLevel) yield break;

            string cleanedText = options.Text.Replace("\r", "");

            if (cleanedText.Any(c => !options.Font.CanPrint(c)))
            {
                throw new ArgumentException("text contains unprintable characters");
            }

            TilePositionCollection tilePositionCollection = new TilePositionCollection(cleanedText.Length);
            Vector3Int startingPosition = new Vector3Int(options.StartingPosition.x, options.StartingPosition.y, 0);
            if (options.CharactersPerSecondScroll.HasValue)
            {
                SetTextAnimationTracker tracker = new SetTextAnimationTracker
                {
                    CleanedText = cleanedText,
                    Timer = 0,
                    Options = options,
                    PositionCursor = startingPosition,
                    TextCursor = 0,
                    TilePositionCollection = tilePositionCollection
                };

                currentAnimations.Add(tracker);
                yield return new WaitWhile(() => currentAnimations.Contains(tracker));
            }
            else
            {
                setTilesToText(cleanedText, tilePositionCollection, ref startingPosition, options);
                yield break;
            }
        }

        public void Delete (DeleteTextOptions options)
        {
            int
                xWidth = Mathf.Abs(options.RegionExtents.x),
                yWidth = Mathf.Abs(options.RegionExtents.y),
                xDir = Math.Sign(options.RegionExtents.x),
                yDir = Math.Sign(options.RegionExtents.y);

            Vector3Int[,] positions2d = new Vector3Int[xWidth, yWidth];
            for (int i = 0; i < xWidth; i++)
            {
                for (int j = 0; j < yWidth; j++)
                {
                    positions2d[i, j] = new Vector3Int(options.RegionStartPosition.x + xDir * i, options.RegionStartPosition.y + yDir * j, 0);
                }
            }

            Vector3Int[] positions = positions2d.Cast<Vector3Int>().ToArray();
            TileBase[] tiles = Enumerable.Repeat<TileBase>(null, xWidth * yWidth).ToArray();

            Driver.CharacterAnimations.StopAnimations(MainTextTilemap, positions.ToList());
            MainTextTilemap.SetTiles(positions, tiles);

            // TODO: animatinos, scrolling deletions
        }

        void animateFrame (SetTextAnimationTracker tracker)
        {
            if (endingLevel) return;

            float charactersPerSecondScroll = tracker.Options.CharactersPerSecondScroll.Value;
            int charactersPerFrame = Mathf.Max(Mathf.RoundToInt(charactersPerSecondScroll * Time.deltaTime), 1);

            string cleanedText = tracker.CleanedText;
            int textToSetLength = Mathf.Min(charactersPerFrame, cleanedText.Length - tracker.TextCursor);
            string textToSet = cleanedText.Substring(tracker.TextCursor, textToSetLength);

            setTilesToText(textToSet, tracker.TilePositionCollection, ref tracker.PositionCursor, tracker.Options);
            tracker.TextCursor += textToSetLength;

            tracker.Timer = 1f / charactersPerSecondScroll;
        }

        void setTilesToText (string text, TilePositionCollection tilePositionCollection, ref Vector3Int tilemapCursor, SetTextOptions data)
        {
            if (endingLevel) return;

            tilePositionCollection.Clear();

            foreach (char c in text)
            {
                switch (c)
                {
                    case '\t':
                        tilemapCursor += Vector3Int.right * data.Font.SpacesPerTab;
                        break;
                    case '\n':
                        tilemapCursor = new Vector3Int(data.StartingPosition.x, tilemapCursor.y - 1, 0);
                        break;
                    default:
                        Tile tile = data.Font.GetPrintableAsciiCharacter(c);
                        tilePositionCollection.Add(tilemapCursor, tile);
                        tilemapCursor += Vector3Int.right;
                        break;
                }
            }

            if (tilePositionCollection.Count > 0)
            {
                // TODO: figure out which tilemap(s) to do
                Tilemap tilemap = MainTextTilemap;

                if (data.LoadAnimation != null)
                {
                    StartCoroutine(Driver.CharacterAnimations.AnimateTileset(data.LoadAnimation, 0, tilemap, tilePositionCollection));
                }
                else
                {
                    tilemap.SetTiles(tilePositionCollection.Positions, tilePositionCollection.Tiles);
                }
            }
        }
    }
}