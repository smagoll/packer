using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

sealed class UIFurnitureSpawnerSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly StaticData staticData;
    private readonly SceneData sceneData;

    private readonly EcsFilter<WalletComponent> walletFilter;
    private readonly EcsFilter<ShowListFurnituresEvent> uiSpawnFurnitureFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;

    public void Run()
    {
        foreach (var i in uiSpawnFurnitureFilter)
        {
            foreach (Transform furniture in sceneData.listFurnitures)
            {
                Object.Destroy(furniture.gameObject);
            }
            
            foreach (var furniture in staticData.furnitures)
            {
                Spawn(furniture);
            }
        }
    }

    private void Spawn(FurnitureIncomeData furniture)
    {
        var furnitureObject = Object.Instantiate(staticData.prefabFurnitureUI, sceneData.listFurnitures);
        
        var storeCell = furnitureObject.GetComponent<StoreCell>();
        storeCell.textPrice.text = furniture.price.ToString();

        furnitureObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            var pos = highlightFilter.Get2(0).position;
            if (!TileExtensions.CheckTile(sceneData.tilemapFurniture, pos))
            {
                BuyFurniture(furniture);
            }
            else
            {
                world.NewEntity().Get<FailBuyEvent>();
            }
        });
    }

    private void BuyFurniture(FurnitureIncomeData furniture)
    {
        var furnitureData = staticData.furnitures.FirstOrDefault(x => x.id == furniture.id);
        ref var money = ref walletFilter.Get1(0).money;
        if (money >= furnitureData.price)
        {
            money -= furnitureData.price;
            var entity = officeOpenFilter.GetEntity(0);
            ref var spawnFurnitureComponent = ref entity.Get<SpawnFurnitureEvent>();
            spawnFurnitureComponent.id = furniture.id;
        }
        else
        {
            world.NewEntity().Get<FailBuyEvent>();
        }
    }
}