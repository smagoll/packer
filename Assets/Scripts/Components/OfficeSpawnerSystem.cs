using Leopotam.Ecs;
using UnityEngine;

sealed class OfficeSpawnerSystem : IEcsInitSystem
{
    private readonly EcsWorld _world = null;
    private StaticData staticData;

    public void Init()
    {
        EcsEntity office = _world.NewEntity();

        ref var officeComponent = ref office.Get<OfficeComponent>();
    }

    private void Spawn()
    {
        Object.Instantiate(staticData.officePrefab);
    }
}