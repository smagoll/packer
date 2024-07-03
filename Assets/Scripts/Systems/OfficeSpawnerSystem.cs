using Leopotam.Ecs;
using UnityEngine;

sealed class OfficeSpawnerSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<SpawnOfficeEvent> spawnOfficeFilter;
    private readonly EcsFilter<OfficeComponent> officeFilter;

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
        var officeObject = Object.Instantiate(staticData.offices[0].prefab, sceneData.officeTransform);
        officeObject.GetComponent<EntityReference>().entity = office;
    }
}