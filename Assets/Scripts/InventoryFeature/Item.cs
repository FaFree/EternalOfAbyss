using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.InventoryFeature
{
    public enum RareType
    {
        Default,
        Rare,
        SuperRare,
        Legendary
    }
    
    [Serializable]
    public class Item : ICloneable
    {
        public string itemId;
        
        public string key;

        public string textInfo;

        public int currentItemLevel;

        public float upgradeCost;
        
        public ItemType itemType;
        
        public ItemStats itemStats;
        
        public bool isEquip;

        public Sprite sprite;

        private Dictionary<StatType, Func<float>> statsMap;

        public Item(string key, float upgradeCost, ItemType itemType, ItemStats itemStats, Sprite sprite)
        {
            this.itemId = Guid.NewGuid().ToString();
            this.key = key;
            this.itemType = itemType;
            this.itemStats = itemStats;
            this.isEquip = false;
            this.currentItemLevel = 1;
            this.sprite = sprite;
            this.upgradeCost = upgradeCost;
            
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

                    if (this.currentItemLevel != this.itemStats.maxLevel)
                    {
                        if (kvp.Key == StatType.Damage)
                            textInfo += $" -> {(int)(itemStats.damage + itemStats.damage * itemStats.upgradePercent / 100)} \n";
                        
                        else if (kvp.Key == StatType.Health)
                            textInfo += $" -> {(int)(itemStats.health + itemStats.health * itemStats.upgradePercent / 100)} \n";
                        
                        else if (kvp.Key == StatType.Speed)
                            textInfo += $" -> {(int)(itemStats.speed + itemStats.speed * itemStats.upgradePercent / 100)} \n";
                        
                        else
                            textInfo += "\n";
                        
                    }
                    else
                    {
                        textInfo += "\n";
                    }
                }
            }

            textInfo += $"CurrentLevel: {this.currentItemLevel} \nMaxLevel: {this.itemStats.maxLevel}";
        }

        public override string ToString()
        {
            UpdateTextInfo();

            return textInfo;
        }

        public void Upgrade()
        {
            itemStats.damage += itemStats.damage * itemStats.upgradePercent / 100;
            itemStats.health += itemStats.health * itemStats.upgradePercent / 100;
            itemStats.speed += itemStats.speed * itemStats.upgradePercent / 100;
            
            upgradeCost += upgradeCost * itemStats.upgradePercent / 100;

            this.currentItemLevel++;

            UpdateTextInfo();
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
        
        public float upgradePercent;
        
        public int maxLevel;
        
        public bool isRangeWeapon;

        public RareType rare;
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