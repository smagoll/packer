using DG.Tweening;
using Leopotam.Ecs;

sealed class EffectSystem : IEcsRunSystem
{
    private readonly SceneData sceneData;
    private readonly EcsFilter<FailBuyEvent> failBuyEventFilter;
    
    public void Run()
    {
        foreach (var i in failBuyEventFilter)
        {
            DOTween.Sequence()
                .Append(sceneData.failBackground.DOFade(.3f, .1f))
                .Append(sceneData.failBackground.DOFade(0, .5f));
        }
    }
}