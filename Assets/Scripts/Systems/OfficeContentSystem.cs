using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Tilemaps;

sealed class OfficeContentSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<SpawnOfficeContentEvent> spawnOfficeContentFilter;
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get1(i);
            PaintTiles(spawnOfficeContentEvent.size);
            
            Debug.Log("content spawn");
        }
    }

    private void PaintTiles(int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                PaintSingleTile(sceneData.tilemapFloor, staticData.tileFloor, i, j);
            }
        }
    }
    
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, float x, float y)
    {
        tilemap.SetTile(new Vector3Int((int)x, (int)y), tile);
    }
}