using System.Linq;
using System.Xml.XPath;
using DefaultNamespace;
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
        sceneData.buttonCreateFurniture.onClick.AddListener(() => AddFurniture(ref officeFilter.Get1(0), 1));
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
                AddFurniture(ref office, furniture.id);
            }
        }
    }
    
    public OfficeComponent AddOffice()
    {
        EcsEntity office = _world.NewEntity();
        ref var officeComponent = ref office.Get<OfficeComponent>();
        office.Get<PositionComponent>();
        Debug.Log("add office");
        return officeComponent;
    }

    public void AddFurniture(ref OfficeComponent office, int id)
    {
        EcsEntity furniture = _world.NewEntity();
        ref var furnitureComponent = ref furniture.Get<FurnitureComponent>();
        ref var incomeComponent = ref furniture.Get<IncomeComponent>();
        furniture.Get<LevelComponent>();

        furnitureComponent.id = id;
        incomeComponent.income = staticData.furnitures.FirstOrDefault(x => x.id == id).income;
        
        if (office.Furnitures == null) office.Furnitures = new();
        office.Furnitures.Add(furnitureComponent);

        EcsEntity entity = _world.NewEntity();
        entity.Get<UpdateIncomeEvent>();
        
        Debug.Log("add furniture");
    }


}