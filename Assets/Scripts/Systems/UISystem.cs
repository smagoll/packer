using Leopotam.Ecs;

sealed class UISystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> uiTransitionOfficeContentEvent;
    private readonly EcsFilter<ShowListFurnituresEvent> showListFurnituresEventFilter;
    private readonly EcsFilter<HideListFurnituresEvent> hideListFurnituresEventFilter;

    public void Init()
    {
        sceneData.buttonBackStoreToMain.onClick.AddListener(ButtonBackStoreToMain);
        sceneData.buttonBackContentToMain.onClick.AddListener(ButtonBackContentToMain);
        sceneData.buttonBuyOffice.onClick.AddListener(ShowStore);
        sceneData.sellFurniture.onClick.AddListener(() => world.NewEntity().Get<SellFurnitureEvent>());
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
        sceneData.canvasMain.gameObject.SetActive(false);
        sceneData.canvasContent.gameObject.SetActive(true);
        sceneData.officeContent.SetActive(true);
    }

    private void ShowListFurnitures()
    {
        if (!sceneData.furniturePanel.gameObject.activeSelf)
        {
            sceneData.furniturePanel.gameObject.SetActive(true);
        }
    }
    
    private void HideListFurnitures()
    {
        if (sceneData.furniturePanel.gameObject.activeSelf)
        {
            sceneData.furniturePanel.gameObject.SetActive(false);
        }
    }

    private void ButtonBackStoreToMain()
    {
        ref var switchEvent = ref world.NewEntity().Get<SwitchMainStoreEvent>();
        switchEvent.isSwitch = false;
    }
    
    private void ButtonBackContentToMain()
    {
        ref var office = ref officeOpenedFilter.GetEntity(0);
        office.Del<Opened>();
        
        HideListFurnitures();
        
        sceneData.canvasMain.gameObject.SetActive(true);
        sceneData.canvasContent.gameObject.SetActive(false);
        sceneData.officeContent.SetActive(false);
        sceneData.tilemapFurniture.ClearAllTiles();
    }

    private void ShowStore()
    {
        world.NewEntity().Get<OpenStoreEvent>();
    }
}