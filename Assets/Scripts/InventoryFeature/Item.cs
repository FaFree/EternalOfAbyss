using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.InventoryFeature

{
    [Serializable]
    public class Item
    {
        public string itemId;
        
        public string key;
        
        public ItemType itemType;
        
        public ItemStats itemStats;
        
        public bool isEquip;

        public Sprite sprite;

        public Item(string key, ItemType itemType, ItemStats itemStats)
        {
            this.itemId = Guid.NewGuid().ToString();
            this.key = key;
            this.itemType = itemType;
            this.itemStats = itemStats;
            this.isEquip = false;
        }
    }

    [Serializable]
    public struct ItemStats
    {
        public float damage;
        
        public float attackRange;
        
        public float health;

        public float speed;

        public float attackSpeed;

        public bool isRangeWeapon;
    }
}