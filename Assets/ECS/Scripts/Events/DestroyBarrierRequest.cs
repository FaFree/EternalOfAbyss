using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct DestroyBarrierRequest : IEventData
    {
        public EntityId entityId;
    }
}