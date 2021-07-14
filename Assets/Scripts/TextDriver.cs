using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Drepanoid
{
    public class TextDriver : MonoBehaviour
    {
        public Tilemap MainTextTilemap, TextShadowTilemap;
        public TextQueue TextQueue;

        void Awake ()
        {
            TextQueue.RegisterProcessors(setTextRoutine, clearTextRoutine);
        }

        IEnumerator setTextRoutine (TextEffectData data)
        {
            string cleanedText = data.Text.Replace("\r", "");

            if (cleanedText.Any(c => !data.Font.CanPrint(c)))
            {
                throw new ArgumentException("text contains unprintable characters");
            }

            Vector3Int tilemapCursor = new Vector3Int(data.StartingPosition.x, data.StartingPosition.y, 0);
            int textCursor = 0;

            float? charactersPerSecond = data.CharactersPerSecondScroll.ToNullable;
            List<CharacterLoadAnimations.TileSpecification> tilesToAdd = new List<CharacterLoadAnimations.TileSpecification>(cleanedText.Length);

            while (textCursor < cleanedText.Length)
            {
                int charactersPerFrame = charactersPerSecond.HasValue
                    ? Mathf.Max(Mathf.RoundToInt(charactersPerSecond.Value * Time.deltaTime), 1)
                    : cleanedText.Length;

                int textToSetLength = Mathf.Min(charactersPerFrame, cleanedText.Length - textCursor);
                string textToSet = cleanedText.Substring(textCursor, textToSetLength);
                textCursor += textToSetLength;

                tilesToAdd.Clear();

                foreach (char c in textToSet)
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
                            tilesToAdd.Add(new CharacterLoadAnimations.TileSpecification { Position = tilemapCursor, Tile = tile });
                            tilemapCursor += Vector3Int.right;
                            break;
                    }
                }

                if (tilesToAdd.Count > 0)
                {
                    // TODO: figure out which tilemap(s) to do
                    Tilemap tilemap = MainTextTilemap;

                    if (data.Animations != null)
                    {
                        // TODO: manage the lifecycle of these animations properly (ie, abort the animation if something else is printed over or if the position gets deleted)
                        StartCoroutine(data.Animations.AnimateTileset(0, tilemap, tilesToAdd, true));
                    }
                    else
                    {
                        tilemap.SetTiles(tilesToAdd.Select(t => t.Position).ToArray(), tilesToAdd.Select(t => t.Tile).ToArray());
                    }
                }

                if (charactersPerSecond.HasValue) yield return new WaitForSeconds(1f / charactersPerSecond.Value);
            }
        }

        void clearTextRoutine (Vector2Int regionStartPosition, Vector2Int regionExtents)
        {
            int
                xWidth = Mathf.Abs(regionExtents.x),
                yWidth = Mathf.Abs(regionExtents.y),
                xDir = Math.Sign(regionExtents.x),
                yDir = Math.Sign(regionExtents.y);

            Vector3Int[,] positions2d = new Vector3Int[xWidth, yWidth];
            for (int i = 0; i < xWidth; i++)
            {
                for (int j = 0; j < yWidth; j++)
                {
                    positions2d[i, j] = new Vector3Int(regionStartPosition.x + xDir * i, regionStartPosition.y + yDir * j, 0);
                }
            }

            Vector3Int[] positions = positions2d.Cast<Vector3Int>().ToArray();
            TileBase[] tiles = Enumerable.Repeat<TileBase>(null, xWidth * yWidth).ToArray();

            MainTextTilemap.SetTiles(positions, tiles);

            // TODO: animatinos, scrolling deletions
        }
    }
}