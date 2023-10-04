using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct DamagedEvent : IEventData
    {
        public EntityId EntityId;
        public float Damage;
    }
}