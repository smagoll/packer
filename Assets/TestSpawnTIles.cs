using UnityEngine;
using UnityEngine.Tilemaps;

public class TestSpawnTIles : MonoBehaviour
{
    private int size = 2;
    
    [SerializeField]
    private Tilemap tilemapFloor;
    [SerializeField]
    private Tilemap tilemapFurniture;
    [SerializeField]
    private Tilemap tilemapHighlight;
    [SerializeField]
    private TileBase tileFloor;
    [SerializeField]
    private TileBase tileHighlight;
    [SerializeField]
    private TileBase tileFurniture;
    
    private void Start()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                PaintSingleTile(tilemapFloor, tileFloor, i, j);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cam = Camera.main;
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            var worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
            
            var tpos = tilemapFloor.WorldToCell(worldPoint);
            
            Debug.Log(tpos);
            
            // Try to get a tile from cell position
            var tile = tilemapFloor.GetTile(tpos);

            if(tile)
            {
                tilemapHighlight.ClearAllTiles();
                PaintSingleTile(tilemapHighlight, tileHighlight, tpos.x , tpos.y);
            }
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, float x, float y)
    {
        tilemap.SetTile(new Vector3Int((int)x, (int)y), tile);
    }
}
