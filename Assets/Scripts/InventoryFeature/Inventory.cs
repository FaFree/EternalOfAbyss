using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Scripts.Events.InventoryEvents;
using Scellecs.Morpeh;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.StorageService;

namespace Scripts.InventoryFeature
{
    public enum ItemType
    {
        Helmet,
        Chest,
        Weapon,
        Ring
    }
    
    public class Inventory
    {
        private const string saveKey = "Inventory";

        private Event<OnItemChanged> onItemChanged;

        private IStorageService storageService;

        private InventorySaver inventorySaver;
        
        public List<Item> Items { get; private set; }

        public Dictionary<ItemType, Item> CurrentItems { get; private set; }

        public Inventory()
        {
            storageService = new JsonFileStorageService();
            
            CurrentItems = new Dictionary<ItemType, Item>();
            Items = new List<Item>();
        }

        public void Initialize()
        {
            this.onItemChanged = World.Default.GetEvent<OnItemChanged>();

            var itemsMap = WorldModels.Default.Get<Items>().ItemsMap;
            this.AddItem(itemsMap["DEFAULT_BOW"]);

            CurrentItems.Add(ItemType.Helmet, default);
            CurrentItems.Add(ItemType.Chest, default);
            CurrentItems.Add(ItemType.Weapon, default);
            CurrentItems.Add(ItemType.Ring, default);

            foreach (var item in Items)
            {
                if (item.isEquip)
                {
                    Equip(item);
                }
            }
        }

        public bool TryRemoveItem(string key)
        {
            foreach (var item in Items)
            {
                if (item.key == key)
                {
                    Items.Remove(item);

                    if (item.isEquip)
                    {
                        CurrentItems[item.itemType] = default;
                    }
                    
                    return true;
                }
            }

            return false;
        }

        public void AddRange(Item[] items)
        {
            this.Items.AddRange(items);
        }
        
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public Item GetItemOrDefault(string key)
        {
            foreach (var item in Items)
            {
                if (item.key == key)
                    return item;
            }

            return default;
        }
        
        public void Equip(Item item)
        {
            if (CurrentItems[item.itemType] != default)
            {
                var lastItem = CurrentItems[item.itemType];
                lastItem.isEquip = false;
            }
            
            CurrentItems[item.itemType] = item;
            item.isEquip = true;
            
            onItemChanged.NextFrame(new OnItemChanged
            {
                itemType = item.itemType,
                itemKey = item.key
            });
        }

        private void Load()
        {
            inventorySaver = storageService.Load<InventorySaver>(saveKey);

            this.Items = inventorySaver.items;

            foreach (var item in Items)
            {
                if (item.isEquip)
                {
                    CurrentItems[item.itemType] = item;
                }
            }
        }
        
        private void Save()
        {
            inventorySaver = new InventorySaver(this.Items);
            
            storageService.Save(saveKey, inventorySaver);
        }
    }

    [Serializable]
    public struct InventorySaver
    {
        public List<Item> items;

        public InventorySaver(List<Item> items)
        {
            this.items = items;
        }
    }
}