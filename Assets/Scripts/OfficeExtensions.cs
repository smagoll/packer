using Leopotam.Ecs;

namespace DefaultNamespace
{
    public static class OfficeExtensions
    {
        public static void AddOffice(EcsWorld world)
        {
            EcsEntity entity = world.NewEntity();
            entity.Get<AddOfficeEvent>();
        }
    }
}