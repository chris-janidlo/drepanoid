using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

public class TilemapLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterLoadAnimation Animation;

    public Tilemap Tilemap;

    class IndividualTileAnimationTracker
    {
        public Vector3Int Position;
        public TileBase FinalTile;
        public int CurrentFrame;
        public float Timer;
        public CharacterLoadAnimation Animation;

        public TileBase CurrentTile => IsFinished ? FinalTile : Animation.Frames[CurrentFrame].Tile;
        public bool IsFinished => CurrentFrame >= Animation.Frames.Count;
    }

    void Start ()
    {
        StartCoroutine(playAnimation());
    }

    IEnumerator playAnimation ()
    {
        List<IndividualTileAnimationTracker> individualAnimationData = new List<IndividualTileAnimationTracker>();

        foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
        {
            var tile = Tilemap.GetTile(cellPosition);
            if (tile == null) continue;

            individualAnimationData.Add(new IndividualTileAnimationTracker
            {
                Position = cellPosition,
                FinalTile = tile,
                CurrentFrame = -1,
                Animation = Animation
            });

            Tilemap.SetTile(cellPosition, null);
        }

        yield return new WaitForSeconds(ShowDelay);

        Vector3Int[] positionArray = new Vector3Int[individualAnimationData.Count];
        TileBase[] tileArray = new TileBase[individualAnimationData.Count];
        int cursor = 0;

        while (individualAnimationData.Count > 0)
        {
            foreach (var anim in individualAnimationData)
            {
                anim.Timer -= Time.deltaTime;
                if (anim.Timer > 0) continue;

                anim.Timer = RandomExtra.Range(Animation.FrameTimeRange);
                anim.CurrentFrame++;

                positionArray[cursor] = anim.Position;
                tileArray[cursor] = anim.CurrentTile;
                cursor++;
            }

            Tilemap.SetTiles(positionArray, tileArray);
            individualAnimationData.RemoveAll(anim => anim.IsFinished);

            yield return null;

            Array.Clear(positionArray, 0, cursor);
            Array.Clear(tileArray, 0, cursor);
            cursor = 0;
        }
    }
}
