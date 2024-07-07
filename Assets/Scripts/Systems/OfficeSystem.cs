using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using YG;

sealed class OfficeSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsWorld _world;
    private readonly StaticData staticData;
    private readonly SceneData sceneData;
    
    private readonly EcsFilter<AddOfficeEvent> addOfficeFilter;
    private readonly EcsFilter<OfficeComponent> officeFilter;
    private readonly EcsFilter<OfficeComponent, SpawnFurnitureEvent, Opened> officeOpenFilter;
    
    public void Init()
    {
        CreateStartOffices(YandexGame.savesData.offices);
    }

    public void Run()
    {
        foreach (var i in addOfficeFilter)
        {
            var id = addOfficeFilter.Get1(i).id;
            CreateOffice(id);
        }

        foreach (var i in officeOpenFilter)
        {
            ref var officeComponent = ref officeOpenFilter.Get1(i);
            ref var spawnFurnitureEvent = ref officeOpenFilter.Get2(i);
            
            CreateFurniture(ref officeComponent, spawnFurnitureEvent.id, new Vector2(0,0));
        }
    }
    
    private void CreateStartOffices(OfficeSave[] officeSaves)
    {
        foreach (var startOffice in officeSaves)
        {
            var office = CreateOffice(startOffice.id);
            ref var officeComponent = ref office.Get<OfficeComponent>();
            foreach (var furniture in startOffice.furnitures)
            {
                CreateFurniture(ref officeComponent, furniture.id, furniture.position);
            }
        }
    }
    
    public EcsEntity CreateOffice(int id)
    {
        EcsEntity office = _world.NewEntity();
        ref var officeComponent = ref office.Get<OfficeComponent>();
        office.Get<SpawnOfficeEvent>();
        officeComponent.id = id;
        return office;
    }

    public void CreateFurniture(ref OfficeComponent office, int id, Vector2 position)
    {
        EcsEntity furniture = _world.NewEntity();
        ref var furnitureComponent = ref furniture.Get<FurnitureComponent>();
        ref var incomeComponent = ref furniture.Get<IncomeComponent>();
        ref var positionComponent = ref furniture.Get<PositionComponent>();
        furniture.Get<LevelComponent>();

        positionComponent.position = position;
        furnitureComponent.id = id;
        incomeComponent.income = staticData.furnitures.FirstOrDefault(x => x.id == id).income;
        
        if (office.furnitures == null) office.furnitures = new();
        office.furnitures.Add(furniture);

        furniture.Get<UpdateIncomeEvent>();
    }
}