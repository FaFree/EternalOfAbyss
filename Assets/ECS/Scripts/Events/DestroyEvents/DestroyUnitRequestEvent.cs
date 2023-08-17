using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct DestroyUnitRequestEvent : IEventData
    {
        public EntityId entityId;
    }
}