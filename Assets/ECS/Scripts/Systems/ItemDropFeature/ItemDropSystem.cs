using System;
using System.Collections.Generic;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Random = UnityEngine.Random;

namespace ECS.Scripts.Components.ItemDropFeature
{
    public class ItemDropSystem : UpdateSystem
    {
        private const int DROP_CHANCE = 50;
        
        private Event<DestroyedUnitEvent> destroyedEvent;
        private Event<ItemDropRequest> dropRequest;

        private Dictionary<string, Item> allItemsMap;

        private List<Item> defaultItems;
        private List<Item> rareItems;
        private List<Item> superRareItems;
        private List<Item> legendaryItems;

        public override void OnAwake()
        {
            this.destroyedEvent = this.World.GetEvent<DestroyedUnitEvent>();
            this.dropRequest = this.World.GetEvent<ItemDropRequest>();

            this.defaultItems = new List<Item>();
            this.rareItems = new List<Item>();
            this.superRareItems = new List<Item>();
            this.legendaryItems = new List<Item>();

            this.allItemsMap = WorldModels.Default.Get<Items>().ItemsMap;

            foreach (var item in allItemsMap)
            {
                switch (item.Value.itemStats.rare)
                {
                    case RareType.Default: defaultItems.Add(item.Value);
                        break;
                    case RareType.Rare: rareItems.Add(item.Value);
                        break;
                    case RareType.SuperRare: superRareItems.Add(item.Value);
                        break;
                    case RareType.Legendary: legendaryItems.Add(item.Value);
                        break;
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (destroyedEvent.IsPublished)
            {
                if (Random.Range(1, 101) <= DROP_CHANCE)
                {
                    string key = GetRandomItemKeyFromList();
                    
                    dropRequest.NextFrame(new ItemDropRequest
                    {
                        ItemKey = key
                    });
                }
            }
        }

        private string GetRandomItemKeyFromList()
        {
            int rare = Random.Range(1, 101);

            if (rare <= 50)
            {
                int id = Random.Range(0, this.defaultItems.Count);
                return this.defaultItems[id].key;
            }
            
            if (rare <= 80)
            {
                int id = Random.Range(0, this.rareItems.Count);
                return this.rareItems[id].key;
            }
            
            if (rare <= 95)
            {
                int id = Random.Range(0, this.superRareItems.Count);
                return this.superRareItems[id].key;
            }

            int Id = Random.Range(0, this.legendaryItems.Count);
            return this.legendaryItems[Id].key;
        }
    }
}