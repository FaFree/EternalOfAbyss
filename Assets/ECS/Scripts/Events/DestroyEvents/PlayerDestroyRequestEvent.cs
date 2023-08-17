using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct PlayerDestroyRequestEvent : IEventData
    {
        public EntityId entityId;
    }
}