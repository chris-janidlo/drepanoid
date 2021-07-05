using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using crass;

public class TilemapLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterLoadAnimations Animation;

    public Tilemap Tilemap;
    public TilemapCollider2D TilemapCollider;

    class IndividualTileAnimationTracker
    {
        public Vector3Int Position;
        public TileBase FinalTile;
        public int CurrentFrame;
        public float Timer;
        public List<CharacterLoadAnimations.AnimationFrame> Frames;

        public TileBase CurrentTile => IsFinished ? FinalTile : Frames[CurrentFrame].Tile;
        public bool IsFinished => CurrentFrame >= Frames.Count;
    }

    void Start ()
    {
        StartCoroutine(playAnimation(true));
    }

    public void OnLevelGoalReached ()
    {
        StartCoroutine(playAnimation(false));
    }

    IEnumerator playAnimation (bool loading)
    {
        if (TilemapCollider != null) TilemapCollider.enabled = false;

        List<CharacterLoadAnimations.TileSpecification> tiles = new List<CharacterLoadAnimations.TileSpecification>();

        foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
        {
            var tile = Tilemap.GetTile(cellPosition);
            if (tile == null) continue;

            tiles.Add(new CharacterLoadAnimations.TileSpecification { Position = cellPosition, Tile = tile, });

            if (loading) Tilemap.SetTile(cellPosition, null);
        }

        yield return Animation.AnimateTileset(ShowDelay, Tilemap, tiles, loading);

        if (TilemapCollider != null && loading) TilemapCollider.enabled = true;
    }
}
