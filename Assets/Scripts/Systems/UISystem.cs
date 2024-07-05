using Leopotam.Ecs;
using UnityEngine;

sealed class UISystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<UITransitionOfficeContentEvent> uiTransitionOfficeContentEvent;
    private readonly EcsFilter<ShowListFurnituresEvent> showListFurnituresEventFilter;
    private readonly EcsFilter<HideListFurnituresEvent> hideListFurnituresEventFilter;
    
    public void Run()
    {
        foreach (var i in uiTransitionOfficeContentEvent)
        {
            TransitionToOfficeContent();
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

    private void TransitionToOfficeContent()
    {
        sceneData.canvasMain.SetActive(false);
        sceneData.officeContent.SetActive(true);
    }

    private void ShowListFurnitures()
    {
        if (!sceneData.canvasContent.activeSelf)
        {
            sceneData.canvasContent.SetActive(true);
            EcsEntity entity = _world.NewEntity();
            entity.Get<UISpawnFurnitureEvent>();
        }
    }
    
    private void HideListFurnitures()
    {
        if (sceneData.canvasContent.activeSelf)
        {
            sceneData.canvasContent.SetActive(false);
        }
    }
}