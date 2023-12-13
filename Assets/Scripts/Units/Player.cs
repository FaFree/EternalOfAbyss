using Scellecs.Morpeh;
using Scripts;
using Scripts.BoostFeature;
using Scripts.InventoryFeature;
using Scripts.StorageService;

namespace DefaultNamespace
{
    public class Player : BaseUnit
    {
        private IStorageService storageService;
        
        public Player(UnitConfig config, float firstAttackTime, float attackAnimationTime) 
            : base(config, firstAttackTime, attackAnimationTime)
        {
            
        }

        public void AddBoost(Boost boost)
        {
            if (boost.category == Categories.Player)
            {
                this.Damage += boost.playerBoostConfig.damage * this.AttackTime;
            }
        }

        public void ChangeWeapon(Item item)
        {
            this.RemoveBoosts();
            
            this.Damage = item.itemStats.damage;
            this.AttackTime = item.itemStats.attackSpeed;

            if (item.isGun)
                this.FirstAttackTime = 0.7f;
            else
            {
                this.FirstAttackTime = 1.01f;
            }

            var boosts = WorldModels.Default.Get<BoostsModel>().boosts;

            foreach (var boost in boosts)
            {
                this.AddBoost(boost);
            }
        }

        public void RemoveBoosts()
        {
            var boosts = WorldModels.Default.Get<BoostsModel>().boosts;

            foreach (var boost in boosts)
            {
                if (boost.category == Categories.Player)
                {
                    this.Damage -= boost.playerBoostConfig.damage;
                }
            }
        }

        private void Save()
        {
            var config = new UnitConfig("", this.Speed, (int)this.MaxHealth, this.AttackRange, this.Damage, this.AttackTime, 0, this.CritChance, this.CritMultipler);
            
            this.storageService.Save("PlayerModel", config);
        }
    }
}