using Leopotam.Ecs;
using UnityEngine;

sealed class UISystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld _world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> uiTransitionOfficeContentEvent;
    private readonly EcsFilter<ShowListFurnituresEvent> showListFurnituresEventFilter;
    private readonly EcsFilter<HideListFurnituresEvent> hideListFurnituresEventFilter;

    public void Init()
    {
        sceneData.buttonBackToMain.onClick.AddListener(ButtonBack);
        sceneData.buttonBuyOffice.onClick.AddListener(ShowStore);
    }
    
    public void Run()
    {
        foreach (var i in uiTransitionOfficeContentEvent)
        {
            TransitionToContent();
            HideListFurnitures();
        }

        foreach (var i in showListFurnituresEventFilter)
        {
            ShowListFurnitures();
        }
        
        foreach (var i in hideListFurnituresEventFilter)
        {
            HideListFurnitures();
        }
    }

    private void TransitionToContent()
    {
        sceneData.canvasMain.SetActive(false);
        sceneData.canvasContent.SetActive(true);
        sceneData.officeContent.SetActive(true);
    }

    private void ShowListFurnitures()
    {
        if (!sceneData.furnitureWindow.activeSelf)
        {
            sceneData.furnitureWindow.SetActive(true);
            EcsEntity entity = _world.NewEntity();
            entity.Get<UISpawnFurnitureEvent>();
        }
    }
    
    private void HideListFurnitures()
    {
        if (sceneData.furnitureWindow.activeSelf)
        {
            sceneData.furnitureWindow.SetActive(false);
        }
    }

    private void ButtonBack()
    {
        ref var office = ref officeOpenedFilter.GetEntity(0);
        office.Del<Opened>();
        
        HideListFurnitures();
        
        sceneData.canvasMain.SetActive(true);
        sceneData.canvasContent.SetActive(false);
        sceneData.officeContent.SetActive(false);
        sceneData.tilemapFurniture.ClearAllTiles();
    }

    private void ShowStore()
    {
        sceneData.storeWindow.SetActive(true);

        _world.NewEntity().Get<OpenStoreEvent>();
    }
}