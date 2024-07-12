using System;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;

sealed class CameraControllerSystem : IEcsRunSystem
{
    private readonly EcsWorld world;

    private readonly EcsFilter<MoveCamera> moveCameraFilter;
    
    private readonly Camera cam = Camera.main;
    private Vector3 startPos;
    private Vector3 worldPoint;
    private Vector3 direction;
    private bool isMove;

    public void Run()
    {
        if (!moveCameraFilter.IsEmpty())
        {
            ref var moveCameraComponent = ref moveCameraFilter.Get1(0);
            startPos = moveCameraComponent.startPos;
            
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            worldPoint = cam.ScreenToWorldPoint(mousePos);
            isMove = true;
            direction = startPos - worldPoint;
            
            if (direction.magnitude > .01f)
            {
                moveCameraComponent.isMove = true;
            }
            else
            {
                direction = Vector3.zero;
            }
        }
        else
        {
            direction = Vector3.Lerp(direction, Vector3.zero, 5f * Time.deltaTime);
        }
        
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (direction.magnitude > 0f)
        {
            cam.transform.position += Vector3.Lerp(Vector3.zero, direction, 10f * Time.deltaTime);
        }
    }
}