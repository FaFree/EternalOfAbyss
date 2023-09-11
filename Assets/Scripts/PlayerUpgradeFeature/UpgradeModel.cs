using System.Collections.Generic;

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

        public void Initialize()
        {
            currentUpgrades = new Dictionary<UpgradeType, Upgrade>
            {
                { UpgradeType.Health, new Upgrade(0, 100, 0)},
                { UpgradeType.Damage, new Upgrade(100, 0, 0)},
                { UpgradeType.Crit, new Upgrade(1, 0, 0)}
            };
        }

        public void ActivateUpgrade(UpgradeType type)
        {
            GetNewUpgrade(type);
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
            
            Upgrade newUpgrade = new Upgrade(damagePercent, healthPercent, critPercent);

            currentUpgrades[type] = newUpgrade;
        }
    }

    public struct Upgrade
    {
        public float DamagePercent;
        public float HealthPercent;
        public float CritPercent;

        public Upgrade (float damagePercent, float healthPercent, float critPercent)
        {
            DamagePercent = damagePercent;
            HealthPercent = healthPercent;
            CritPercent = critPercent;
        }
    }
}