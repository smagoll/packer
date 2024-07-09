using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

sealed class ContentSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnOfficeContentFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<FurnitureComponent, PositionComponent, Selled> furnitureSelledFilter;
    private readonly EcsFilter<SellFurnitureEvent> sellFurnitureFilter;
    private readonly EcsFilter<WalletComponent> walletFilter;
    private readonly EcsFilter<ShowEditPanelEvent> showEditPanel;
    private readonly EcsFilter<HideEditPanelEvent> hideEditPanel;

    private ButtonCell buttonCell;
    
    public void Init()
    {
        buttonCell = sceneData.sellFurniture.gameObject.GetComponent<ButtonCell>();
    }
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get2(i);
            PaintTiles(spawnOfficeContentEvent.size);
        }
        
        foreach (var i in sellFurnitureFilter)
        {
            SellFurniture();
        }

        foreach (var i in showEditPanel)
        {
            MarkFurniture();
        }

        foreach (var i in hideEditPanel)
        {
            foreach (var j in furnitureSelledFilter)
            {
                furnitureSelledFilter.GetEntity(j).Del<Selled>();
            }
        }
    }

    private void PaintTiles(int size)
    {
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
    }

    private void MarkFurniture()
    {
        ref var officeComponent = ref officeOpenedFilter.Get1(0);
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