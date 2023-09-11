using Scripts;
using Scellecs.Morpeh;
using Scripts.PlayerUpgradeFeature;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitPlayer : BaseUnit
    {
        public UnitPlayer(UnitConfig config, float firstAttackTime, float attackAnimationTime) 
            : base(config, firstAttackTime, attackAnimationTime)
        {

        }

        public void UpgradeModel(UpgradeType type)
        {
            var upgradeModel = WorldModels.Default.Get<UpgradeModel>();

            var upgrade = upgradeModel.GetCurrentUpgrade(type);

            this.damageWithoutItem += this.damageWithoutItem * (upgrade.DamagePercent / 100);
            this.healthWithoutItem += this.healthWithoutItem * (upgrade.HealthPercent / 100);
            this.critWithoutItem += this.critWithoutItem * (upgrade.CritPercent / 100);

            this.ChangeItem();
        }
    }
}