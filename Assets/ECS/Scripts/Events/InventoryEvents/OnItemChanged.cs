using Scellecs.Morpeh;
using Scripts.InventoryFeature;

namespace ECS.Scripts.Events.InventoryEvents
{
    public struct OnItemChanged : IEventData
    {
        public string itemKey;
        public ItemType itemType;
    }
}