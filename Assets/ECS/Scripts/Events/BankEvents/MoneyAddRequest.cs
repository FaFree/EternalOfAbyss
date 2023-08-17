using Scellecs.Morpeh;

namespace ECS.Scripts.Events.BankEvents
{
    public struct MoneyAddRequest : IEventData
    {
        public int count;
    }
}