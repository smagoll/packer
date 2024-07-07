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
        var officeObject = Object.Instantiate(staticData.offices[0].prefab, sceneData.listOffices);
        officeObject.GetComponent<EntityReference>().entity = office;
        
        officeObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ref var officeContentEvent = ref office.Get<SpawnContentEvent>();
            office.Get<Opened>();
            officeContentEvent.size = 2;
        });
    }
}