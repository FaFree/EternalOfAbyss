using Scripts;
using Scellecs.Morpeh;
using Scripts.PlayerUpgradeFeature;
using Scripts.StorageService;
using UnityEngine;
using UnityEngine.TestTools;

namespace DefaultNamespace
{
    public class UnitPlayer : BaseUnit
    {
        private IStorageService storageService;
        
        public UnitPlayer(UnitConfig config, float firstAttackTime, float attackAnimationTime) 
            : base(config, firstAttackTime, attackAnimationTime)
        {
            
        }

        public void UpgradeModel(UpgradeType type)
        {
            storageService = new JsonFileStorageService();

            var upgradeModel = WorldModels.Default.Get<UpgradeModel>();

            var upgrade = upgradeModel.GetCurrentUpgrade(type);

            this.damageWithoutItem += this.damageWithoutItem * (upgrade.DamagePercent / 100);
            this.healthWithoutItem += this.healthWithoutItem * (upgrade.HealthPercent / 100);
            this.critWithoutItem += this.critWithoutItem * (upgrade.CritPercent / 100);
            
            upgradeModel.ActivateUpgrade(type);

            this.ChangeItems();

            UnitConfig config = new UnitConfig("", this.speedWithoutItem, (int)this.healthWithoutItem,
                this.attackRangeWithouItem, this.damageWithoutItem, this.AttackTime, 0, 0, this.critWithoutItem,
                this.critMultiplerWithoutItem);
            
            storageService.Save("PlayerModel", config);
        }
    }
}