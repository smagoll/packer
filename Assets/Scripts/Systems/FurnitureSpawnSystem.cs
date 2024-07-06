using Leopotam.Ecs;
using UnityEngine;
using YG;
using Vector2 = System.Numerics.Vector2;

sealed class FurnitureSpawnSystem : IEcsRunSystem
{
    private readonly EcsFilter<HighlightTileComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<SpawnFurnitureEvent> spawnFurnitureFilter;
    private readonly EcsFilter<OfficeComponent, SpawnFurnitureStartEvent> spawnFurnitureStartFilter;
    
    public void Run()
    {
        foreach (var i in spawnFurnitureStartFilter)
        {
            var office = spawnFurnitureStartFilter.GetEntity(i);
            foreach (var furnitureComponent in office.Get<OfficeComponent>().Furnitures)
            {
                //Spawn();
            }
        }
        
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

    private void Spawn(int id, Vector2 position)
    {
        Debug.Log("spawn start");
    }
}