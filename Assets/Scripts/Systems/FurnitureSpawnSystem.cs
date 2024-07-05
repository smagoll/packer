using Leopotam.Ecs;
using UnityEngine;

sealed class FurnitureSpawnSystem : IEcsRunSystem
{
    private readonly EcsFilter<HighlightTileComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<SpawnFurnitureEvent> spawnFurnitureFilter;
    
    public void Run()
    {
        foreach (var i in spawnFurnitureFilter)
        {
            ref var spawnEvent = ref spawnFurnitureFilter.Get1(i);
            Spawn(spawnEvent.id);
        }
    }

    private void Spawn(int id)
    {
        var positionComponent = highlightFilter.Get2(0);
        
        
        Debug.Log("spawn " + id);
        Debug.Log("position " + positionComponent.position);
    }
}