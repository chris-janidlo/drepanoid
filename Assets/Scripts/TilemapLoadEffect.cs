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
        List<IndividualTileAnimationTracker> animationData = new List<IndividualTileAnimationTracker>();

        foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
        {
            var tile = Tilemap.GetTile(cellPosition);
            if (tile == null) continue;

            animationData.Add(new IndividualTileAnimationTracker
            {
                Position = cellPosition,
                FinalTile = tile,
                CurrentFrame = -1,
                Animation = Animation
            });

            Tilemap.SetTile(cellPosition, null);
        }

        yield return new WaitForSeconds(ShowDelay);

        Vector3Int[] positionArray = new Vector3Int[animationData.Count];
        TileBase[] tileArray = new TileBase[animationData.Count];
        int cursor = 0;

        while (animationData.Count > 0)
        {
            foreach (var data in animationData)
            {
                data.Timer -= Time.deltaTime;
                if (data.Timer > 0) continue;

                data.Timer = RandomExtra.Range(Animation.FrameTimeRange);
                data.CurrentFrame++;

                positionArray[cursor] = data.Position;
                tileArray[cursor] = data.CurrentTile;
                cursor++;
            }

            Tilemap.SetTiles(positionArray, tileArray);
            animationData.RemoveAll(anim => anim.IsFinished);

            yield return null;

            Array.Clear(positionArray, 0, cursor);
            Array.Clear(tileArray, 0, cursor);
            cursor = 0;
        }
    }
}
