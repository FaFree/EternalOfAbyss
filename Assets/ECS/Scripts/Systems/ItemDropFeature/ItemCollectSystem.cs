using System.Collections.Generic;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;

namespace ECS.Scripts.Components.ItemDropFeature
{
    public class ItemCollectSystem : UpdateSystem
    {
        private Event<ItemDropRequest> itemDropRequest;
        private Event<ItemDroppedEvent> itemDroppedEvent;

        private Dictionary<string, Item> allItemsMap;

        private Inventory inventory;
        public override void OnAwake()
        {
            this.itemDropRequest = this.World.GetEvent<ItemDropRequest>();
            this.itemDroppedEvent = this.World.GetEvent<ItemDroppedEvent>();
            
            this.allItemsMap = WorldModels.Default.Get<Items>().ItemsMap;
            this.inventory = WorldModels.Default.Get<Inventory>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.itemDropRequest.IsPublished)
                return;

            foreach (var evt in this.itemDropRequest.BatchedChanges)
            {
                var item = this.allItemsMap[evt.ItemKey];

                Item newItem = new Item(item.key, item.upgradeCost, item.itemType, item.itemStats, item.sprite);
                
                this.inventory.AddItem(newItem);
                
                this.itemDroppedEvent.NextFrame(new ItemDroppedEvent
                {
                    ItemId = newItem.itemId
                });
            }
        }
    }
}