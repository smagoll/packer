using Leopotam.Ecs;
using UnityEngine;

sealed class HighlightTileSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    private readonly EcsFilter<HighlightTileEvent> highlightTileEventFilter;
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnOfficeContentFilter;

    private readonly SceneData sceneData;
    private readonly StaticData staticData;

    private EcsEntity highlightEntity;
    
    public void Init()
    {
        highlightEntity = world.NewEntity();
        highlightEntity.Get<PositionComponent>();
        highlightEntity.Get<HighlightComponent>();
    }
    
    public void Run()
    {
        foreach (var i in highlightTileEventFilter)
        {
            ref var highlightComponent = ref highlightTileEventFilter.Get1(i);
            var worldPoint = highlightComponent.worldPoint;
            
            var tilePos = sceneData.tilemapFloor.WorldToCell(worldPoint);
            var tile = sceneData.tilemapFloor.GetTile(tilePos);

            if(tile)
            { 
                SetHighlight(tilePos);
            }
            else
            {
                ClearHighlight();
            }
        }
        
        foreach (var i in spawnOfficeContentFilter)
        {
            ClearHighlight();
        }
    }

    private void ClearHighlight()
    {
        sceneData.tilemapHighlight.ClearAllTiles();
        highlightEntity.Get<HideListFurnituresEvent>();
    }

    private void SetHighlight(Vector3Int position)
    {
        sceneData.tilemapHighlight.ClearAllTiles();
        TileExtensions.PaintSingleTile(sceneData.tilemapHighlight, staticData.tileHighlight, position.x , position.y);

        if (TileExtensions.CheckTile(sceneData.tilemapFurniture, position))
        {
            highlightEntity.Get<HideListFurnituresEvent>();
            sceneData.editPanel.SetActive(true);
        }
        else
        {
            highlightEntity.Get<ShowListFurnituresEvent>();
            sceneData.editPanel.SetActive(false);
        }
        
        ref var positionComponent = ref highlightEntity.Get<PositionComponent>();
        positionComponent.position = position;
    }
}