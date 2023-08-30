using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.InventoryFeature.InventoryModel
{
    public class Items : ScriptableObject
    {
        [SerializeField] private List<Item> items;

        public Dictionary<string, Item> ItemsMap { get; private set; }

        public void Initialize()
        {
            this.ItemsMap = items.ToDictionary(x => x.key, x => (Item) x.Clone());

            foreach (var item in ItemsMap)
            {
                item.Value.itemId = Guid.NewGuid().ToString();
                
                item.Value.Initialize();
            }
        }
    }
}