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
        Pants,
        Boots,
        Ring
    }
    
    public class Inventory
    {
        private const string saveKey = "Inventory";

        private Event<OnItemChanged> onItemChanged;

        private IStorageService storageService;

        private InventorySaver inventorySaver;
        
        public List<Item> AllItems { get; private set; }
        public List<Item> InventoryItems { get; private set; }

        public Dictionary<ItemType, Item> CurrentItems { get; private set; }

        public Inventory()
        {
            storageService = new JsonFileStorageService();
            
            CurrentItems = new Dictionary<ItemType, Item>();
            InventoryItems = new List<Item>();
            AllItems = new List<Item>();
        }

        public void Initialize()
        {
            this.onItemChanged = World.Default.GetEvent<OnItemChanged>();

            var itemsMap = WorldModels.Default.Get<Items>().ItemsMap;
            
            this.AddItem(itemsMap["DEFAULT_BOW"]);
            this.AddItem(itemsMap["CHEST01"]);
            this.AddItem(itemsMap["DEFAULT_WEAPON"]);

            CurrentItems.Add(ItemType.Helmet, default);
            CurrentItems.Add(ItemType.Chest, default);
            CurrentItems.Add(ItemType.Weapon, default);
            CurrentItems.Add(ItemType.Pants, default);
            CurrentItems.Add(ItemType.Boots, default);
            CurrentItems.Add(ItemType.Ring, default);

            foreach (var item in AllItems)
            {
                if (item.isEquip)
                {
                    Equip(item);
                }
            }
        }

        public bool TryRemoveItem(string key)
        {
            foreach (var item in InventoryItems)
            {
                if (item.key == key)
                {
                    InventoryItems.Remove(item);

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
            this.InventoryItems.AddRange(items);
        }
        
        public void AddItem(Item item)
        {
            InventoryItems.Add(item);
            AllItems.Add(item);
        }

        public Item GetItemOrDefault(string key)
        {
            foreach (var item in AllItems)
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
                InventoryItems.Add(lastItem);
            }
            
            CurrentItems[item.itemType] = item;
            item.isEquip = true;
            InventoryItems.Remove(item);
            
            onItemChanged.NextFrame(new OnItemChanged
            {
                itemType = item.itemType,
                itemKey = item.key
            });
        }

        public void UnEquip(ItemType itemType)
        {
            var lastItem = CurrentItems[itemType];
            
            if (lastItem != default)
            {
                lastItem.isEquip = false;
                
                InventoryItems.Add(lastItem);
                CurrentItems[itemType] = default;
            }
        }

        private void Load()
        {
            inventorySaver = storageService.Load<InventorySaver>(saveKey);

            this.InventoryItems = inventorySaver.items;

            foreach (var item in InventoryItems)
            {
                if (item.isEquip)
                {
                    CurrentItems[item.itemType] = item;
                    InventoryItems.Remove(item);
                }
            }
        }
        
        private void Save()
        {
            List<Item> items = new List<Item>();
            
            items.AddRange(this.InventoryItems.ToArray());

            foreach (var item in CurrentItems)
            {
                items.Add(item.Value);
            }
            
            inventorySaver = new InventorySaver(items);
            
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