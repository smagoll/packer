using Leopotam.Ecs;
using UnityEditor.UI;

sealed class UISystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<FurnitureComponent, Selled> furnitureSelledFilter;
    private readonly EcsFilter<HighlightComponent, PositionComponent> highlightFilter;
    
    private readonly EcsFilter<OfficeComponent, SpawnContentEvent> uiTransitionOfficeContentEvent;
    
    private readonly EcsFilter<ShowListFurnituresEvent> showListFurnituresEventFilter;
    private readonly EcsFilter<HideListFurnituresEvent> hideListFurnituresEventFilter;
    
    private readonly EcsFilter<ShowEditPanelEvent> showEditPanelFilter;
    private readonly EcsFilter<HideEditPanelEvent> hideEditPanelFilter;

    private ButtonCell buttonCell;
    
    public void Init()
    {
        sceneData.buttonBackStoreToMain.onClick.AddListener(ButtonBackStoreToMain);
        sceneData.buttonBackContentToMain.onClick.AddListener(ButtonBackContentToMain);
        sceneData.buttonBuyOffice.onClick.AddListener(ShowStore);
        sceneData.sellFurniture.onClick.AddListener(() => world.NewEntity().Get<SellFurnitureEvent>());

        buttonCell = sceneData.sellFurniture.GetComponent<ButtonCell>();
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

        foreach (var i in showEditPanelFilter)
        {
            sceneData.editPanel.SetActive(true);
            sceneData.furniturePanel.SetActive(false);
            
            UpdateTextButton();
        }
        
        foreach (var i in hideEditPanelFilter)
        {
            sceneData.editPanel.SetActive(false);
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
    
    private void UpdateTextButton()
    {
        var priceCell = furnitureSelledFilter.Get2(0).price;
        buttonCell.textPrice.text = priceCell.ToString();
    }
}