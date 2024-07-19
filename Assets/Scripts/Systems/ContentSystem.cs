using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using YG;

sealed class ContentSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnOfficeContentFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<FurnitureComponent, PositionComponent, Selled> furnitureSelledFilter;
    private readonly EcsFilter<SellFurnitureEvent> sellFurnitureEventFilter;
    private readonly EcsFilter<WalletComponent> walletFilter;
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get2(i);
            PaintTiles(spawnOfficeContentEvent.size);
        }
        
        foreach (var i in sellFurnitureEventFilter)
        {
            SellFurniture();
        }
    }

    private void PaintTiles(int size)
    {
        sceneData.tilemapFloor.ClearAllTiles();
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                TileExtensions.PaintSingleTile(sceneData.tilemapFloor, staticData.tileFloor, i, j);
            }
        }
    }
    
    private void SellFurniture()
    {
        ref var officeComponent = ref officeOpenedFilter.Get1(0);
        var furniture = furnitureSelledFilter.GetEntity(0);
        var position = furniture.Get<PositionComponent>().position;
        
        sceneData.tilemapFurniture.SetTile(Vector3Int.FloorToInt(position), null);

        var priceSell = furniture.Get<Selled>().price;
        ref var walletComponent = ref walletFilter.Get1(0);
        walletComponent.money += priceSell;
        
        world.NewEntity().Get<UpdateIncomeEvent>();

        officeComponent.furnitures.Remove(furniture);
        furniture.Destroy();
        
        world.NewEntity().Get<HideEditPanelEvent>();
        sceneData.tilemapHighlight.ClearAllTiles();
        DeleteFurnitureData(officeComponent.id, position);
    }

    private void DeleteFurnitureData(int idOffice, Vector2 position)
    {
        var officeSave = YandexGame.savesData.offices.First(x => x.id == idOffice);
        officeSave.furnitures.RemoveAll(x => x.position == position);
    }
}