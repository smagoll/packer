using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileExtensions
{
    public static void PaintSingleTile(Tilemap tilemap, TileBase tile, float x, float y)
    {
        tilemap.SetTile(new Vector3Int((int)x, (int)y), tile);
    }

    public static bool CheckTile(Tilemap tilemap, Vector3 position)
    {
        var pos = Vector3Int.FloorToInt(position);
        var tile = tilemap.GetTile(pos);
        return tile;
    }
}