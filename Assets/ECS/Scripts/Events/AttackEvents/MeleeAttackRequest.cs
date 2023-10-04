using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct MeleeAttackRequest : IEventData
    {
        public EntityId entityId;
    }
}