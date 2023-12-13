using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.InventoryFeature
{
    [Serializable]
    public class Item : ICloneable
    {
        public string itemId;
        
        public string key;

        public string textInfo;

        public ItemType itemType;
        
        public ItemStats itemStats;

        public bool isGun;
        
        public bool isEquip;

        public string ammoKey;

        public Sprite sprite;

        private Dictionary<StatType, Func<float>> statsMap;

        public Item(string key, ItemType itemType, ItemStats itemStats, Sprite sprite)
        {
            this.itemId = Guid.NewGuid().ToString();
            this.key = key;
            this.itemType = itemType;
            this.itemStats = itemStats;
            this.isEquip = false;
            this.sprite = sprite;
            
            this.Initialize();
        }

        public void Initialize()
        {
            statsMap = new Dictionary<StatType, Func<float>>()
            {
                { StatType.Damage, () => itemStats.damage },
                { StatType.Health, () => itemStats.health },
                { StatType.Speed, () => itemStats.speed },
                { StatType.AttackSpeed, () => itemStats.speed },
                { StatType.AttackRange, () => itemStats.attackRange }
            };

            UpdateTextInfo();
        }

        private void UpdateTextInfo()
        {
            textInfo = "";

            foreach (var kvp in statsMap)
            {
                if (kvp.Value() > 0)
                {
                    textInfo += $"{kvp.Key.ToString()}: {(int) kvp.Value()}";
                    
                    textInfo += "\n";
                }
            }
        }

        public override string ToString()
        {
            UpdateTextInfo();

            return textInfo;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
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
    }

    public enum StatType
    {
        Damage,
        Health,
        Speed,
        AttackSpeed,
        AttackRange,
    }
}