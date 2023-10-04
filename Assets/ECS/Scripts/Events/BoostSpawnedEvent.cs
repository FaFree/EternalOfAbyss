using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct BoostSpawnedEvent : IEventData
    {
        public Boost Boost;
        public EntityId EntityId;
    }
}