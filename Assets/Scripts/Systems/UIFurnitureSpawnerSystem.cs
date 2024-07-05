using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

sealed class UIFurnitureSpawnerSystem : IEcsRunSystem
{
    private readonly EcsWorld _world;
    private readonly StaticData staticData;
    private readonly SceneData sceneData;

    private readonly EcsFilter<UISpawnFurnitureEvent> uiSpawnFurnitureFilter;

    public void Run()
    {
        foreach (var i in uiSpawnFurnitureFilter)
        {
            foreach (Transform furniture in sceneData.listFurnitures)
            {
                Object.Destroy(furniture.gameObject);
            }
            foreach (var furniture in staticData.furnitures)
            {
                Spawn(furniture);
                Debug.Log($"spawn {furniture.title}");  
            }
        }
    }

    private void Spawn(FurnitureIncomeData furniture)
    {
        var furnitureObject = Object.Instantiate(staticData.furnitures[0].prefab, sceneData.listFurnitures);
        furnitureObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            EcsEntity entity = _world.NewEntity();
            ref var spawnFurnitureComponent = ref entity.Get<SpawnFurnitureEvent>();
            spawnFurnitureComponent.id = furniture.id;
        });
    }
}