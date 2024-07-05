using Leopotam.Ecs;

sealed class HighlightTileSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly EcsFilter<HighlightTileEvent> highlightTileEventFilter;

    private readonly SceneData sceneData;
    private readonly StaticData staticData;
    
    public void Run()
    {
        foreach (var i in highlightTileEventFilter)
        {
            ref var highlightComponent = ref highlightTileEventFilter.Get1(i);
            var worldPoint = highlightComponent.worldPoint;
            
            var tpos = sceneData.tilemapFloor.WorldToCell(worldPoint);

            // Try to get a tile from cell position
            var tile = sceneData.tilemapFloor.GetTile(tpos);

            if(tile)
            { 
                sceneData.tilemapHighlight.ClearAllTiles();
                TileExtensions.PaintSingleTile(sceneData.tilemapHighlight, staticData.tileHighlight, tpos.x , tpos.y);

                EcsEntity entity = world.NewEntity();
                entity.Get<ShowListFurnituresEvent>();
            }
            else
            {
                sceneData.tilemapHighlight.ClearAllTiles();
                
                EcsEntity entity = world.NewEntity();
                entity.Get<HideListFurnituresEvent>();
            }
        }
    }
}