using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

sealed class CameraControllerSystem : IEcsRunSystem
{
    private readonly EcsWorld world;

    private readonly SceneData sceneData;
    
    private readonly EcsFilter<MouseDownInput> mouseDownFilter;
    private readonly EcsFilter<MouseUpInput> mouseUpFilter;
    
    private readonly Camera cam = Camera.main;
    private Vector3 startPos;
    private Vector3 worldPoint;
    private Vector3 direction;
    private bool isMove;
    private bool gotMoving;

    private TileBase selectedTile;
    
    public void Run()
    {
        if (!mouseDownFilter.IsEmpty())
        {
            if(CheckUI()) return;
            
            sceneData.tilemapHighlight.ClearAllTiles();
            world.NewEntity().Get<HideListFurnituresEvent>();
            sceneData.editPanel.SetActive(false);
            
            var cam = Camera.main;
            var mousePos = Input.mousePosition;
            
            if (cam != null)
            {
                mousePos.z = -cam.transform.position.z;
                var worldPoint = cam.ScreenToWorldPoint(mousePos);

                var tpos = sceneData.tilemapFloor.WorldToCell(worldPoint);
                // Try to get a tile from cell position
                var tile = sceneData.tilemapFloor.GetTile(tpos);

                if (tile)
                {
                    selectedTile = tile;
                }

                startPos = worldPoint;
                isMove = true;
            }
        }

        if (!mouseUpFilter.IsEmpty())
        {
            var localMove = gotMoving;
            isMove = false;
            gotMoving = false;
            if (localMove) return;
            
            if(CheckUI()) return;
            
            var cam = Camera.main;
            var mousePos = Input.mousePosition;
            
            if (cam != null)
            {
                mousePos.z = -cam.transform.position.z;
                var worldPoint = cam.ScreenToWorldPoint(mousePos);

                var tilePosition = sceneData.tilemapFloor.WorldToCell(worldPoint);

                var tile = sceneData.tilemapFloor.GetTile(tilePosition);

                if (tile)
                {
                    if (selectedTile == tile)
                    {
                        EcsEntity entity = world.NewEntity();
                        ref var highlightEvent = ref entity.Get<HighlightTileEvent>();
                        highlightEvent.worldPoint = worldPoint;
                    }
                }
                else
                {
                    sceneData.tilemapHighlight.ClearAllTiles();
                    world.NewEntity().Get<HideListFurnituresEvent>();
                    sceneData.editPanel.SetActive(false);
                }
            }
            
            selectedTile = null;
        }
        
        UpdateDirection();
        MoveCamera();
    }

    private void UpdateDirection()
    {
        if (isMove)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            worldPoint = cam.ScreenToWorldPoint(mousePos);
            direction = startPos - worldPoint;
            
            if (direction.magnitude > .01f)
            {
                gotMoving = true;
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
    }
    
    private void MoveCamera()
    {
        if (direction.magnitude > 0f)
        {
            cam.transform.position += Vector3.Lerp(Vector3.zero, direction, 10f * Time.deltaTime);
            var x = Mathf.Clamp(cam.transform.position.x, -10, 10);
            var y = Mathf.Clamp(cam.transform.position.y, -10, 10);
            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }
    }
    
    private bool CheckUI()
    {
        var eventSystem = EventSystem.current;
        var pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI")) return true;
        }
        
        return false;
    }
}