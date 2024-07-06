using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

sealed class FurnitureSpawnSystem : IEcsRunSystem
{
    private readonly StaticData staticData;
    private readonly SceneData sceneData;
    
    private readonly EcsFilter<HighlightTileComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<OfficeComponent, SpawnFurnitureEvent, Opened> spawnFurnitureFilter;
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnFurnitureStartFilter;
    
    public void Run()
    {
        foreach (var i in spawnFurnitureStartFilter)
        {
            var officeComponent = spawnFurnitureStartFilter.Get1(i);
            ref var furnitures = ref officeComponent.furnitures;

            if (furnitures == null) return;
            
            foreach (var furnitureEntity in furnitures)
            {
                var position = furnitureEntity.Get<PositionComponent>().position;
                var id = furnitureEntity.Get<FurnitureComponent>().id;
                Spawn(id, new Vector2(position.x, position.y));
            }
        }
        
        foreach (var i in spawnFurnitureFilter)
        {
            ref var spawnEvent = ref spawnFurnitureFilter.Get2(i);
            var positionComponent = highlightFilter.Get2(0);
            var position = positionComponent.position;
            
            Spawn(spawnEvent.id , new Vector2(position.x, position.y));
        }
    }

    private void Spawn(int id, Vector2 position)
    {
        var furniture = staticData.furnitures.FirstOrDefault(x => x.id == id);
        TileExtensions.PaintSingleTile(sceneData.tilemapFurniture, furniture.prefabTile, position.x, position.y);
    }
}