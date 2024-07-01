using Leopotam.Ecs;
using UnityEngine;

sealed class OfficeSpawnerSystem : IEcsInitSystem
{
    private readonly EcsWorld _world = null;
    private StaticData staticData;

    public void Init()
    {
        Spawn(new Vector2(1,1));
    }

    private void Spawn(Vector2 position)
    {
        Object.Instantiate(staticData.officePrefab);
    }
}