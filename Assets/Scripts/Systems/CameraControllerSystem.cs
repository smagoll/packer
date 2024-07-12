using System;
using Leopotam.Ecs;
using UnityEngine;

sealed class CameraControllerSystem : IEcsRunSystem
{
    private readonly EcsWorld world;

    private readonly EcsFilter<MoveCamera> moveCameraEventFilter;
    
    private readonly Camera cam = Camera.main;
    private Vector3 startPos;
    private Vector3 direction;

    public void Run()
    {
        if (!moveCameraEventFilter.IsEmpty())
        {
            ref var moveCameraComponent = ref moveCameraEventFilter.Get1(0);
            startPos = moveCameraComponent.startPos;
            
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            var worldPoint = cam.ScreenToWorldPoint(mousePos);
            //direction = startPos - worldPoint;
            direction = worldPoint;
            
            //if (moveCameraComponent.isMove)
            //{
            //    cam.transform.position += Vector3.Lerp(Vector3.zero, direction, 5f * Time.deltaTime);
            //    return;
            //}
            
            if (direction.magnitude > .05f)
            {
                moveCameraComponent.isMove = true;
            }
        }
        
        if(startPos != Vector3.zero) cam.transform.position = Vector3.Lerp(startPos, new Vector3(direction.x, direction.y, cam.transform.position.z), 5f * Time.deltaTime);
    }
}