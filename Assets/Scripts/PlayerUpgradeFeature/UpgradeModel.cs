using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerUpgradeFeature
{
    public enum UpgradeType
    {
        Health,
        Damage,
        Crit
    }
    
    public class UpgradeModel
    {
        private const float LINEAR_FACTOR = 0.2f;
        
        private Dictionary<UpgradeType, Upgrade> currentUpgrades;

        public void Initialize(List<Upgrade> upgrades)
        {
            currentUpgrades = new Dictionary<UpgradeType, Upgrade>();

            foreach (var upgrade in upgrades)
            {
                currentUpgrades.Add(upgrade.upgradeType, upgrade);
            }
        }

        public void ActivateUpgrade(UpgradeType type)
        {
            GetNewUpgrade(type);
        }

        public Dictionary<UpgradeType, Upgrade> GetAllUpgrades()
        {
            return currentUpgrades;
        }

        public Upgrade GetCurrentUpgrade(UpgradeType type)
        {
            return currentUpgrades[type];
        }

        private void GetNewUpgrade(UpgradeType type)
        {
            var upgrade = GetCurrentUpgrade(type);
            
            var damagePercent = upgrade.DamagePercent + upgrade.DamagePercent * LINEAR_FACTOR;
            var healthPercent = upgrade.HealthPercent + upgrade.HealthPercent * LINEAR_FACTOR;
            var critPercent = upgrade.CritPercent + upgrade.CritPercent * LINEAR_FACTOR;
            
            var price = upgrade.upgradePrice + upgrade.upgradePrice * LINEAR_FACTOR;

            currentUpgrades[type].upgradePrice = price;
            currentUpgrades[type].CritPercent = critPercent;
            currentUpgrades[type].HealthPercent = healthPercent;
            currentUpgrades[type].DamagePercent = damagePercent;
        }
    }

    [Serializable]
    public class Upgrade
    {
        public float DamagePercent;
        public float HealthPercent;
        public float CritPercent;
        public float upgradePrice;

        public UpgradeType upgradeType;

        public Sprite upgradeSprite;

        public string GetPercentToString()
        {
            if (DamagePercent > 0)
            {
                var damage = (int)DamagePercent;

                return damage.ToString() + "%";
            }

            if (HealthPercent > 0)
            {
                var health = (int)HealthPercent;

                return health.ToString() + "%";
            }

            var crit = (int)CritPercent;

            return crit.ToString() + "%";
        }
    }
}