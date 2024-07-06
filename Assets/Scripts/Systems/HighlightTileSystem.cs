using Leopotam.Ecs;

sealed class HighlightTileSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    private readonly EcsFilter<HighlightTileEvent> highlightTileEventFilter;

    private readonly SceneData sceneData;
    private readonly StaticData staticData;

    private EcsEntity highlightEntity;
    
    public void Init()
    {
        CreateHighlight();
    }
    
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
                
                highlightEntity.Get<ShowListFurnituresEvent>();
                highlightEntity.Get<PositionComponent>().position = tpos;
            }
            else
            {
                sceneData.tilemapHighlight.ClearAllTiles();
                
                highlightEntity.Get<HideListFurnituresEvent>();
            }
        }
    }

    private void CreateHighlight()
    {
        highlightEntity = world.NewEntity();
        highlightEntity.Get<PositionComponent>();
        highlightEntity.Get<HighlightTileComponent>();
    }
}