using Leopotam.Ecs;
using UnityEngine;

sealed class OfficeSpawnerSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<SpawnOfficeEvent> spawnOfficeFilter;
    private readonly EcsFilter<OfficeComponent> officeFilter;

    public void Init()
    {
        SpawnAll();
    }

    public void Run()
    {
        foreach (var i in spawnOfficeFilter)
        {
            Spawn();
        }
    }
    
    private void SpawnAll()
    {
        foreach (var i in officeFilter)
        {
            Object.Instantiate(staticData.officePrefab, sceneData.officeTransform);
        }
    }
    
    private void Spawn()
    {
        var office = Object.Instantiate(staticData.officePrefab, sceneData.officeTransform);
    }
}