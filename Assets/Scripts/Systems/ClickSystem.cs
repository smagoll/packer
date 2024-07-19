using Leopotam.Ecs;

sealed class ClickSystem : IEcsRunSystem
{
    private readonly EcsFilter<MouseDownInput> mouseDownFilter;
    
    public void Run()
    {
        if (!mouseDownFilter.IsEmpty())
        {
            
        }
    }
}