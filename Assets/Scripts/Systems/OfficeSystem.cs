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
        sceneData.buttonCreateFurniture.onClick.AddListener(() => CreateFurniture(ref officeFilter.Get1(0), 1));
        sceneData.buttonCreateOffice.onClick.AddListener(() => _world.NewEntity().Get<AddOfficeEvent>());
    }

    public void Run()
    {
        foreach (var i in addOfficeFilter)
        {
            CreateOffice(addOfficeFilter.Get1(i).position);
        }
    }
    
    private void CreateStartOffices(OfficeSave[] officeSaves)
    {
        foreach (var startOffice in officeSaves)
        {
            var office = CreateOffice(startOffice.position);
            foreach (var furniture in startOffice.furnitures)
            {
                CreateFurniture(ref office, furniture.id);
            }
        }
    }
    
    public OfficeComponent CreateOffice(Vector2 position)
    {
        EcsEntity office = _world.NewEntity();
        ref var officeComponent = ref office.Get<OfficeComponent>();
        ref var positionComponent = ref office.Get<PositionComponent>();
        office.Get<SpawnOfficeEvent>();

        positionComponent.position = position;
        
        Debug.Log("add office");
        return officeComponent;
    }

    public void CreateFurniture(ref OfficeComponent office, int id)
    {
        EcsEntity furniture = _world.NewEntity();
        ref var furnitureComponent = ref furniture.Get<FurnitureComponent>();
        ref var incomeComponent = ref furniture.Get<IncomeComponent>();
        furniture.Get<LevelComponent>();

        furnitureComponent.id = id;
        incomeComponent.income = staticData.furnitures.FirstOrDefault(x => x.id == id).income;
        
        if (office.Furnitures == null) office.Furnitures = new();
        office.Furnitures.Add(furnitureComponent);

        furniture.Get<UpdateIncomeEvent>();
        
        Debug.Log("add furniture");
    }


}