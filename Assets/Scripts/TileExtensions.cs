using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileExtensions
{
    public static void PaintSingleTile(Tilemap tilemap, TileBase tile, float x, float y)
    {
        tilemap.SetTile(new Vector3Int((int)x, (int)y), tile);
    }
}