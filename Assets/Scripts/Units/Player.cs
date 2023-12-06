using Scellecs.Morpeh;
using Scripts;
using Scripts.BoostFeature;
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
                this.MaxHealth += boost.health;
                this.Damage += boost.damage;
            }
        }

        public void RemoveBoosts()
        {
            var boosts = WorldModels.Default.Get<BoostsModel>().boosts;

            foreach (var boost in boosts)
            {
                if (boost.category == Categories.Player)
                {
                    this.MaxHealth -= boost.health;
                    this.Damage -= boost.damage;
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