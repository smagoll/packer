using System;
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
    
    public void Init()
    {
        CreateStartOffices(YandexGame.savesData.offices);
    }

    public void Run()
    {
        foreach (var i in addOfficeFilter)
        {
            AddOffice();
        }
    }
    
    private void CreateStartOffices(OfficeSave[] officeSaves)
    {
        foreach (var startOffice in officeSaves)
        {
            var office = AddOffice();
            foreach (var furniture in startOffice.furnitures)
            {
                AddFurniture(office, furniture.id);
            }
        }
    }
    
    public OfficeComponent AddOffice()
    {
        EcsEntity office = _world.NewEntity();
        var officeComponent = office.Get<OfficeComponent>();
        office.Get<PositionComponent>();
        Debug.Log("add office");
        return officeComponent;
    }

    public void AddFurniture(OfficeComponent office, int id)
    {
        EcsEntity furniture = _world.NewEntity();
        ref var furnitureComponent = ref furniture.Get<FurnitureComponent>();
        ref var incomeComponent = ref furniture.Get<IncomeComponent>();
        furniture.Get<LevelComponent>();

        furnitureComponent.id = id;
        incomeComponent.income = staticData.furnitures.FirstOrDefault(x => x.id == id).income;
        
        if (office.Furnitures == null) office.Furnitures = new();
        office.Furnitures.Add(furnitureComponent);
        
        Debug.Log("count: " + office.Furnitures.Count);
    }


}