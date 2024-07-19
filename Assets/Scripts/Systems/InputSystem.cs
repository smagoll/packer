using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

sealed class InputSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly SceneData sceneData;

    private readonly EcsFilter<MoveCamera> moveCameraFilter;
    
    private TileBase selectedTile;
    
    public void Run()
    {
        if (Input.GetMouseButtonDown(0))
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

                var entityCam = world.NewEntity();
                ref var moveCameraEvent = ref entityCam.Get<MoveCamera>();
                moveCameraEvent.startPos = worldPoint;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (var i in moveCameraFilter)
            {
                bool isMove = moveCameraFilter.Get1(i).isMove;
                moveCameraFilter.GetEntity(i).Del<MoveCamera>();
                if (isMove) return;
            }
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