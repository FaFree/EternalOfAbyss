using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct PlayerDieRequestEvent : IEventData
    {
        public EntityId entityId;
    }
}