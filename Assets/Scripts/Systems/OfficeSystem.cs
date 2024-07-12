using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;
using YG;

sealed class OfficeSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly StaticData staticData;
    private readonly SceneData sceneData;
    
    private readonly EcsFilter<AddOfficeEvent> addOfficeFilter;
    private readonly EcsFilter<OfficeComponent> officeFilter;
    private readonly EcsFilter<OfficeComponent, SpawnFurnitureEvent, Opened> officeOpenFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    
    public void Init()
    {
        CreateStartOffices(YandexGame.savesData.offices);
    }

    public void Run()
    {
        foreach (var i in addOfficeFilter)
        {
            var officeType = addOfficeFilter.Get1(i).officeType;
            int id = 0;
            if (YandexGame.savesData.offices.Count > 0)
            {
                id = YandexGame.savesData.offices.Max(x => x.id) + 1;
            }
            CreateOffice(id, officeType);
        }

        foreach (var i in officeOpenFilter)
        {
            ref var officeComponent = ref officeOpenFilter.Get1(i);
            ref var spawnFurnitureEvent = ref officeOpenFilter.Get2(i);
            var pos = highlightFilter.Get2(0).position;
            
            CreateFurniture(ref officeComponent, spawnFurnitureEvent.id, pos);
        }
    }
    
    private void CreateStartOffices(List<OfficeSave> officeSaves)
    {
        foreach (var startOffice in officeSaves)
        {
            var office = CreateOffice(startOffice.id, startOffice.officeType);
            ref var officeComponent = ref office.Get<OfficeComponent>();
            foreach (var furniture in startOffice.furnitures)
            {
                CreateFurniture(ref officeComponent, furniture.id, furniture.position);
            }
        }

        world.NewEntity().Get<EndCreateOfficesEvent>();
    }
    
    private EcsEntity CreateOffice(int id, OfficeType officeType)
    {
        EcsEntity office = world.NewEntity();
        ref var officeComponent = ref office.Get<OfficeComponent>();
        office.Get<SpawnOfficeEvent>();
        officeComponent.id = id;
        officeComponent.officeType = officeType;
        
        if(!YandexGame.savesData.offices.Exists(x => x.id == id))
        {
            ref var offices = ref YandexGame.savesData.offices;
            offices.Add(new(id, officeType));
            Debug.Log("save");
        };
        
        return office;
    }

    private void CreateFurniture(ref OfficeComponent office, int id, Vector2 position)
    {
        EcsEntity furniture = world.NewEntity();
        ref var furnitureComponent = ref furniture.Get<FurnitureComponent>();
        ref var incomeComponent = ref furniture.Get<IncomeComponent>();
        ref var positionComponent = ref furniture.Get<PositionComponent>();
        furniture.Get<LevelComponent>();

        positionComponent.position = position;
        furnitureComponent.id = id;
        incomeComponent.income = staticData.furnitures.FirstOrDefault(x => x.id == id).income;
        
        if (office.furnitures == null) office.furnitures = new();
        office.furnitures.Add(furniture);

        var idOffice = office.id; 
        var officeSave = YandexGame.savesData.offices.First(x => x.id == idOffice);
        if(!officeSave.furnitures.Exists(x => x.position == position))
        {
            officeSave.furnitures.Add(new FurnitureSave(id, position));
        }
        
        furniture.Get<UpdateIncomeEvent>();
    }
}