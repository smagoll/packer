using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

sealed class HighlightTileSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    
    private readonly EcsFilter<HighlightTileEvent> highlightTileEventFilter;
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnOfficeContentFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<FurnitureComponent, PositionComponent, Selled> furnitureSelledFilter;

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
        highlightEntity.Get<HideEditPanelEvent>();
    }

    private void SetHighlight(Vector3Int position)
    {
        sceneData.tilemapHighlight.ClearAllTiles();
        TileExtensions.PaintSingleTile(sceneData.tilemapHighlight, staticData.tileHighlight, position.x , position.y);

        if (TileExtensions.CheckTile(sceneData.tilemapFurniture, position))
        {
            highlightEntity.Get<HideListFurnituresEvent>();
            highlightEntity.Get<ShowEditPanelEvent>();
        }
        else
        {
            highlightEntity.Get<ShowListFurnituresEvent>();
            highlightEntity.Get<HideEditPanelEvent>();
        }
        
        ref var positionComponent = ref highlightEntity.Get<PositionComponent>();
        positionComponent.position = position;
        
        MarkFurniture();
    }
    
    private void MarkFurniture()
    {
        foreach (var i in furnitureSelledFilter) furnitureSelledFilter.GetEntity(i).Del<Selled>();
        
        ref var officeComponent = ref officeOpenedFilter.Get1(0);
        if (officeComponent.furnitures == null) return;
        
        ref var listFurnitures = ref officeComponent.furnitures;
        var position = highlightFilter.Get2(0).position;
        
        foreach (var furniture in listFurnitures.ToList())
        {
            if (furniture.Get<PositionComponent>().position == position)
            {
                var furnitureIncomeData = staticData.furnitures.First(x => x.id == furniture.Get<FurnitureComponent>().id);
                var priceSell = Mathf.RoundToInt(furnitureIncomeData.price / 10);
                ref var selled = ref furniture.Get<Selled>();
                selled.price = priceSell;
            }
        }
    }
}