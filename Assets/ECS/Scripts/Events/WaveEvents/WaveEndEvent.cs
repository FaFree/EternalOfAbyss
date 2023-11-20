using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct WaveEndEvent : IEventData
    {
        public EntityId waveEntityId;
    }
}