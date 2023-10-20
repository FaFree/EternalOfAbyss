using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct BaseDieRequestEvent : IEventData
    {
        public EntityId entityId;
    }
}