using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

sealed class InputSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly SceneData sceneData;

    private EcsEntity entityCam;

    private TileBase selectedTile;
    
    public void Run()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(CheckUI()) return;
            
            var cam = Camera.main;
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            var worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

            var tpos = sceneData.tilemapFloor.WorldToCell(worldPoint);
            // Try to get a tile from cell position
            var tile = sceneData.tilemapFloor.GetTile(tpos);

            if (tile)
            {
                selectedTile = tile;
            }
            
            //entityCam = world.NewEntity();
            //entityCam.Get<MoveCameraEvent>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(CheckUI()) return;
            
            var cam = Camera.main;
            var mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            var worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

            var tpos = sceneData.tilemapFloor.WorldToCell(worldPoint);
            // Try to get a tile from cell position
            var tile = sceneData.tilemapFloor.GetTile(tpos);

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

            selectedTile = null;
            //if (entityCam != null) entityCam.Destroy();
        }
    }

    private bool CheckUI()
    {
        var eventSystem = EventSystem.current;
        var pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI")) return true;
        }
        
        return false;
    }
}