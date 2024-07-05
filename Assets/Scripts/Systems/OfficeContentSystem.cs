using Leopotam.Ecs;

sealed class OfficeContentSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<SpawnOfficeContentEvent> spawnOfficeContentFilter;
    
    public void Run()
    {
        foreach (var i in spawnOfficeContentFilter)
        {
            var spawnOfficeContentEvent = spawnOfficeContentFilter.Get1(i);
            PaintTiles(spawnOfficeContentEvent.size);
            
            spawnOfficeContentFilter.GetEntity(i).Get<UITransitionOfficeContentEvent>();
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