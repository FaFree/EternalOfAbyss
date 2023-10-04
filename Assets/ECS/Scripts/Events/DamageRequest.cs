using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct DamageRequest : IEventData
    {
        public float Damage;
        public EntityId EntityId;
    }
}