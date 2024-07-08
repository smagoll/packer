using Leopotam.Ecs;
using UnityEngine;

sealed class ContentSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> spawnOfficeContentFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<DeleteFurnitureEvent> deleteFurnitureFilter;
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get2(i);
            PaintTiles(spawnOfficeContentEvent.size);
        }

        foreach (var i in deleteFurnitureFilter)
        {
            ButtonSell();
        }
    }

    private void PaintTiles(int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                TileExtensions.PaintSingleTile(sceneData.tilemapFloor, staticData.tileFloor, i, j);
            }
        }
    }
    
    private void ButtonSell()
    {
            ref var officeComponent = ref officeOpenedFilter.Get1(0);
            ref var listFurnitures = ref officeComponent.furnitures;
            var position = highlightFilter.Get2(0).position;
            foreach (var furniture in listFurnitures)
            {
                if (furniture.Get<PositionComponent>().position == position)
                {
                    Debug.Log("delete furniture");
                }
            }
    }
}