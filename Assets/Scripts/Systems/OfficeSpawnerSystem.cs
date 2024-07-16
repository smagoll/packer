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
        var officeComponent = office.Get<OfficeComponent>();
        var officeView = officeObject.GetComponent<OfficeView>();
        ref var officeViewReference = ref office.Get<OfficeViewReference>();
        officeViewReference.officeView = officeView;
        var officeData = staticData.offices.FirstOrDefault(x => x.officeType == officeComponent.officeType);
        officeView.icon.sprite = officeData.icon;
        officeView.idText.text = officeComponent.id.ToString();
        string income = officeComponent.furnitures?.Sum(x => x.Get<IncomeComponent>().income).GetReduceMoney();
        officeView.incomeText.text = income;
        
        officeObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ref var officeContentEvent = ref office.Get<SpawnContentEvent>();
            office.Get<Opened>();
            officeContentEvent.size = 2;
        });
    }
}