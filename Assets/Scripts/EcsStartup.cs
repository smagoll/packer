using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class EcsStartup : MonoBehaviour
{
    [SerializeField]
    private StaticData staticData;
    [SerializeField]
    private SceneData sceneData;
    
    public EcsWorld world;
    public EcsSystems systems;

    private void Start()
    {
        world = new();
        systems = new EcsSystems(world);

        systems.ConvertScene();
        
        AddInjections();
        AddSystems();
        AddOneFrames();
        
        systems.Init();
    }

    private void AddSystems()
    {
        systems
            .Add(new InputSystem())
            .Add(new OfficeSystem())
            .Add(new OfficeSpawnerSystem())
            .Add(new FurnitureSpawnSystem())
            .Add(new ContentSystem())
            .Add(new IncomeSystem())
            
            .Add(new HighlightTileSystem())
            .Add(new UISystem())
            .Add(new UIFurnitureSpawnerSystem())
            .Add(new CameraControllerSystem());
    }

    private void AddInjections()
    {
        systems
            .Inject(staticData)
            .Inject(sceneData);
    }

    private void AddOneFrames()
    {
        systems
            .OneFrame<AddOfficeEvent>()
            .OneFrame<UpdateIncomeEvent>()
            .OneFrame<SpawnOfficeEvent>()
            .OneFrame<SpawnFurnitureEvent>()
            .OneFrame<SpawnContentEvent>()
            .OneFrame<HighlightTileEvent>()
            .OneFrame<UITransitionOfficeContentEvent>()
            .OneFrame<ShowListFurnituresEvent>()
            .OneFrame<HideListFurnituresEvent>()
            .OneFrame<UISpawnFurnitureEvent>();
    }
    
    private void Update()
    {
        systems?.Run();
    }

    private void OnDestroy()
    {
        if (systems != null)
        {
            world.Destroy();
            world = null;
            systems.Destroy();
            systems = null;
        }
    }
}