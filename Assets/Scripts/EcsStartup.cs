using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;
using Voody.UniLeo;

public class EcsStartup : MonoBehaviour
{
    [SerializeField]
    private StaticData staticData;
    
    public EcsWorld world;
    public EcsSystems systems;

    private void Start()
    {
        world = new();
        systems = new EcsSystems(world);

        systems.ConvertScene();
        
        AddInjections();
        AddOneFrames();
        AddSystems();
        
        systems.Init();
    }

    private void AddSystems()
    {
        systems
            .Add(new OfficeSystem())
            .Add(new OfficeSpawnerSystem())
            .Add(new FurnitureSpawnerSystem())
            .Add(new IncomeSystem());
    }

    private void AddInjections()
    {
        systems
            .Inject(staticData);
    }
    
    private void AddOneFrames(){}
    
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