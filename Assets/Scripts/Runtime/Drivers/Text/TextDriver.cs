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
            public string CleanedText => CleanedArguments.Text;

            public SetTextArguments CleanedArguments;
            public float Timer;
            public SetTextOptions? Options;
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

        public IEnumerator SetText (SetTextArguments args, SetTextOptions? options = null)
        {
            if (endingLevel) yield break;

            string cleanedText = args.Text.Replace("\r", "");
            SetTextArguments cleanedArguments = new SetTextArguments
            {
                Text = cleanedText,
                Font = args.Font,
                StartingPosition = args.StartingPosition
            };

            if (cleanedText.Any(c => !args.Font.CanPrint(c)))
            {
                throw new ArgumentException("text contains unprintable characters");
            }

            TilePositionCollection tilePositionCollection = new TilePositionCollection(cleanedText.Length);
            Vector3Int startingPosition = new Vector3Int(args.StartingPosition.x, args.StartingPosition.y, 0);
            if (options?.CharactersPerSecondScroll != null)
            {
                SetTextAnimationTracker tracker = new SetTextAnimationTracker
                {
                    CleanedArguments = cleanedArguments,
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
                setTilesToText(cleanedArguments, tilePositionCollection, ref startingPosition, options);
                yield break;
            }
        }

        public IEnumerator SetText (string text, TilesetFont font, Vector2Int startingPosition, SetTextOptions? options = null) => SetText(new SetTextArguments(text, font, startingPosition), options);

        public IEnumerator Delete (DeleteTextArguments arguments, DeleteTextOptions? options = null)
        {
            int
                xWidth = Mathf.Abs(arguments.RegionExtents.x),
                yWidth = Mathf.Abs(arguments.RegionExtents.y),
                xDir = Math.Sign(arguments.RegionExtents.x),
                yDir = Math.Sign(arguments.RegionExtents.y);

            TilePositionCollection tiles = new TilePositionCollection(xWidth * yWidth);

            for (int i = 0; i < xWidth; i++)
            {
                for (int j = 0; j < yWidth; j++)
                {
                    Vector3Int position = new Vector3Int
                    (
                        arguments.RegionStartPosition.x + xDir * i,
                        arguments.RegionStartPosition.y + yDir * j,
                        0
                    );
                    tiles.Add(position, null);
                }
            }

            Driver.CharacterAnimations.StopAnimations(MainTextTilemap, tiles.Positions.ToList());

            if (options?.Animation != null)
            {
                yield return Driver.CharacterAnimations.AnimateTileset(options?.Animation, 0, MainTextTilemap, tiles);
            }
            else
            {
                MainTextTilemap.SetTiles(tiles.Positions, tiles.Tiles);
            }

            // TODO: scrolling deletions
        }

        public IEnumerator Delete (Vector2Int regionStartPosition, Vector2Int regionExtents, DeleteTextOptions? options = null) => Delete(new DeleteTextArguments(regionStartPosition, regionExtents), options);

        void animateFrame (SetTextAnimationTracker tracker)
        {
            if (endingLevel) return;

            int charactersPerSecondScroll = (int) tracker.Options?.CharactersPerSecondScroll; // there should always be a value for this at this point, so we want to immediately throw an exception if not
            int charactersPerFrame = Mathf.Max(Mathf.RoundToInt(charactersPerSecondScroll * Time.deltaTime), 1);

            string cleanedText = tracker.CleanedText;
            int textToSetLength = Mathf.Min(charactersPerFrame, cleanedText.Length - tracker.TextCursor);

            SetTextArguments args = tracker.CleanedArguments;
            args.Text = cleanedText.Substring(tracker.TextCursor, textToSetLength);

            setTilesToText(args, tracker.TilePositionCollection, ref tracker.PositionCursor, tracker.Options);
            tracker.TextCursor += textToSetLength;

            tracker.Timer = 1f / charactersPerSecondScroll;
        }

        void setTilesToText (SetTextArguments args, TilePositionCollection tilePositionCollection, ref Vector3Int tilemapCursor, SetTextOptions? options)
        {
            if (endingLevel) return;

            tilePositionCollection.Clear();

            foreach (char c in args.Text)
            {
                switch (c)
                {
                    case '\t':
                        tilemapCursor += Vector3Int.right * args.Font.SpacesPerTab;
                        break;
                    case '\n':
                        tilemapCursor = new Vector3Int(args.StartingPosition.x, tilemapCursor.y - 1, 0);
                        break;
                    default:
                        Tile tile = args.Font.GetPrintableAsciiCharacter(c);
                        tilePositionCollection.Add(tilemapCursor, tile);
                        tilemapCursor += Vector3Int.right;
                        break;
                }
            }

            if (tilePositionCollection.Count > 0)
            {
                // TODO: figure out which tilemap(s) to do
                Tilemap tilemap = MainTextTilemap;

                if (options?.LoadAnimation != null)
                {
                    StartCoroutine(Driver.CharacterAnimations.AnimateTileset(options?.LoadAnimation, 0, tilemap, tilePositionCollection));
                }
                else
                {
                    tilemap.SetTiles(tilePositionCollection.Positions, tilePositionCollection.Tiles);
                }
            }
        }
    }
}