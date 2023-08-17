using Scripts;
using Scellecs.Morpeh;

namespace ECS.Scripts.Events
{
    public struct UnitSpawnRequest : IEventData
    {
        public Entity zone;
        
        public UnitConfig config;
    }
}