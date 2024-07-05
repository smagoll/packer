using Leopotam.Ecs;
using UnityEngine;

sealed class UISystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    private StaticData staticData;
    private SceneData sceneData;

    private readonly EcsFilter<UITransitionOfficeContentEvent> uiTransitionOfficeContentEvent;
    
    public void Run()
    {
        foreach (var i in uiTransitionOfficeContentEvent)
        {
            TransitionToOfficeContent();
        }
    }

    private void TransitionToOfficeContent()
    {
        sceneData.canvasMain.SetActive(false);
        sceneData.officeContent.SetActive(true);
    }
}