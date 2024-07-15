using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

sealed class OfficeSpawnerSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, SpawnOfficeEvent> spawnOfficeFilter;

    private Transform listOffices;
    
    public void Run()
    {
        foreach (var i in spawnOfficeFilter)
        {
            ref var office = ref spawnOfficeFilter.GetEntity(i);
            Spawn(office);
        }
    }
    
    private void Spawn(EcsEntity office)
    {
        var officeObject = Object.Instantiate(staticData.prefabOffice, sceneData.listOffices);
        officeObject.GetComponent<EntityReference>().entity = office;
        var officeComponent = office.Get<OfficeComponent>();
        var officeView = officeObject.GetComponent<OfficeView>();
        var officeData = staticData.offices.FirstOrDefault(x => x.officeType == officeComponent.officeType);
        officeView.icon.sprite = officeData.icon;
        officeView.idText.text = officeComponent.id.ToString();
        officeView.incomeText.text = officeComponent.furnitures.Sum(x => x.Get<IncomeComponent>().income).ToString();
        
        officeObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ref var officeContentEvent = ref office.Get<SpawnContentEvent>();
            office.Get<Opened>();
            officeContentEvent.size = 2;
        });
    }
}