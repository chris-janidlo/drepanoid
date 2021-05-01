using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLoadEffect : MonoBehaviour
{
    public float ShowDelay;
    public CharacterLoadAnimation Animation;

    public Tilemap Tilemap;

    void Start ()
    {
        foreach (var cellPosition in Tilemap.cellBounds.allPositionsWithin)
        {
            var tile = Tilemap.GetTile(cellPosition);
            if (tile == null) continue;
            StartCoroutine(Animation.DoAnimation(ShowDelay, frame => Tilemap.SetTile(cellPosition, frame.Tile), resultTile: tile));
        }
    }
}
