using Leopotam.Ecs;

sealed class OfficeContentSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, SpawnOfficeContentEvent> spawnOfficeContentFilter;
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get2(i);
            PaintTiles(spawnOfficeContentEvent.size);
            
            var office = spawnOfficeContentFilter.GetEntity(i);
            office.Get<UITransitionOfficeContentEvent>();
            office.Get<SpawnFurnitureStartEvent>();
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
}