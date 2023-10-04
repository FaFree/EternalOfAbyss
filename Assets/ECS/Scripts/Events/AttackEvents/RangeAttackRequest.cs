using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct RangeAttackRequest : IEventData
    {
        public EntityId entityId;
    }
}