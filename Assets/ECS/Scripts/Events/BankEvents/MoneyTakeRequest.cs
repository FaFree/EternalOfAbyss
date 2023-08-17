using Scellecs.Morpeh;

namespace ECS.Scripts.Events.BankEvents
{
    public struct MoneyTakeRequest : IEventData
    {
        public int count;
        public EntityId entityID;
    }
}