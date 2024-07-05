using Leopotam.Ecs;
using UnityEngine;

sealed class CameraControllerSystem : IEcsRunSystem
{
    private readonly EcsWorld world;

    private readonly EcsFilter<MoveCameraEvent> moveCameraEventFilter;
    
    private Camera cam = Camera.main;

    public void Run()
    {
        if (!moveCameraEventFilter.IsEmpty())
        {
            //var mousePos = Input.mousePosition;
            //mousePos.z = -cam.transform.position.z;
            //var pos = Camera.main.ScreenToWorldPoint(mousePos) - startPos;
            //cam.transform.position = new Vector3(pos.x, pos.y, cam.transform.position.z);
            Debug.Log("move camera");
        }
    }
}