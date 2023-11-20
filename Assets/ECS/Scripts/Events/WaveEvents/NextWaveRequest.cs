using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct NextWaveRequest : IEventData
    {
        public EntityId zoneEntityId;
    }
}