using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

sealed class EffectSystem : IEcsRunSystem
{
    private readonly SceneData sceneData;
    private readonly StaticData staticData;
    private readonly EcsFilter<FailBuyEvent> failBuyEventFilter;
    private readonly EcsFilter<OfficeComponent, Opened> officeOpenedFilter;
    
    public void Run()
    {
        if (!officeOpenedFilter.IsEmpty())
        {
            var imgMaterial = sceneData.backgroundContent.material;
            imgMaterial.mainTextureOffset = staticData.offsetParallaxBackground * Time.time;
        }
        
        foreach (var i in failBuyEventFilter)
        {
            AudioController.instance.PlaySFX(AudioController.instance.fail);
            
            DOTween.Sequence()
                .Append(sceneData.failBackground.DOFade(.3f, .1f))
                .Append(sceneData.failBackground.DOFade(0, .5f));
        }
    }
}