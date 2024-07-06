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
    
    public void Init()
    {
        CreateStartOffices(YandexGame.savesData.offices);
        sceneData.buttonCreateOffice.onClick.AddListener(() => _world.NewEntity().Get<AddOfficeEvent>());
    }

    public void Run()
    {
        foreach (var i in addOfficeFilter)
        {
            CreateOffice();
        }
    }
    
    private void CreateStartOffices(OfficeSave[] officeSaves)
    {
        foreach (var startOffice in officeSaves)
        {
            var office = CreateOffice();
            foreach (var furniture in startOffice.furnitures)
            {
                CreateFurniture(ref office, furniture.id, furniture.position);
            }
        }
    }
    
    public OfficeComponent CreateOffice()
    {
        EcsEntity office = _world.NewEntity();
        ref var officeComponent = ref office.Get<OfficeComponent>();
        office.Get<SpawnOfficeEvent>();
        officeComponent.id = 1;
        
        Debug.Log("add office");
        return officeComponent;
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
        
        if (office.Furnitures == null) office.Furnitures = new();
        office.Furnitures.Add(furniture);

        furniture.Get<UpdateIncomeEvent>();
        
        Debug.Log("add furniture");
    }


}