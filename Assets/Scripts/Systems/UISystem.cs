using System.Linq;
using Leopotam.Ecs;
using Unity.VisualScripting;

sealed class UISystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly EcsWorld world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    private readonly EcsFilter<OfficeComponent, OfficeViewReference> officeViewFilter;
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
        sceneData.buttonJob.endClick.AddListener(ShowJobWindow);
        sceneData.buttonOffices.endClick.AddListener(ShowOffice);
        sceneData.buttonBackStoreToMain.endClick.AddListener(ButtonBackStoreToMain);
        sceneData.buttonBackContentToMain.endClick.AddListener(ButtonBackContentToMain);
        sceneData.buttonBuyOffice.endClick.AddListener(ShowStore);
        sceneData.sellFurniture.endClick.AddListener(() =>
        {
            world.NewEntity().Get<SellFurnitureEvent>();
            AudioController.instance.PlaySFX(AudioController.instance.sell);
        });

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
        UpdateIncomeTextOffices();
    }
    
    private void ButtonBackContentToMain()
    {
        ref var office = ref officeOpenedFilter.GetEntity(0);
        office.Del<Opened>();
        
        HideListFurnitures();
        UpdateIncomeTextOffices();
        
        sceneData.canvasMain.gameObject.SetActive(true);
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
        buttonCell.textPrice.text = priceCell.GetReduceMoney();
    }
    
    private void UpdateIncomeTextOffices()
    {
        foreach (var i in officeViewFilter)
        {
            var officeView = officeViewFilter.Get2(i).officeView;
            var officeComponent = officeViewFilter.Get1(i);
            officeView.incomeText.text = officeComponent.furnitures?.Sum(x => x.Get<IncomeComponent>().income).GetReduceMoney();
        }
    }

    private void ShowJobWindow()
    {
        sceneData.job.SetActive(true);
        sceneData.canvasMain.SetActive(false);
    }
    
    private void ShowOffice()
    {
        sceneData.job.SetActive(false);
        sceneData.canvasMain.SetActive(true);
    }
}