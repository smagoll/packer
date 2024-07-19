using Leopotam.Ecs;
using UnityEngine;

sealed class InputSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    
    public void Run()
    {
        if (Input.GetMouseButtonDown(0))
        {
            world.NewEntity().Get<MouseDownInput>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            world.NewEntity().Get<MouseUpInput>();
        }
    }
}