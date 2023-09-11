using System;
using System.Collections.Generic;
using Scripts;
using Scripts.InventoryFeature;

namespace DefaultNamespace
{
    public abstract class BaseUnit
    {
        protected float healthWithoutItem;
        protected float damageWithoutItem;
        protected float attackRangeWithouItem;
        protected float speedWithoutItem;
        protected float critWithoutItem;
        protected float critMultiplerWithoutItem;
        
        protected float attackAnimationTime;
        protected float firstAttackTime;

        public float CritChance { get; private set; }
        public float CritMultipler { get; private set; }
        
        public float MaxHealth { get; private set; }

        public float AttackRange { get; private set; }
        
        public float Damage { get; private set; }
        
        public float Speed { get; private set; }
        
        public float AttackTime { get; private set; }
        public float FirstAttackTime { get; private set; }
        
        public float AnimationAttackTime { get; private set; }
        
        public BaseUnit(UnitConfig config, float firstAttackTime, float attackAnimationTime)
        {
            this.MaxHealth = config.maxHealth;
            this.Damage = config.damage;
            this.AttackRange = config.attackRange;
            this.Speed = config.speed;
            this.AttackTime = config.attackTime;
            this.CritChance = config.critChance;
            this.CritMultipler = config.critMultipler;
            
            this.AnimationAttackTime = attackAnimationTime / this.AttackTime;
            this.FirstAttackTime = firstAttackTime / AnimationAttackTime;

            this.damageWithoutItem = Damage;
            this.healthWithoutItem = MaxHealth;
            this.speedWithoutItem = Speed;
            this.attackRangeWithouItem = AttackRange;
            this.critWithoutItem = this.CritChance;
            this.critMultiplerWithoutItem = this.CritMultipler;

            this.attackAnimationTime = attackAnimationTime;
            this.firstAttackTime = firstAttackTime;
        }

        public virtual bool CanAttack(float range)
        {
            return Math.Pow(this.AttackRange, 2) >= range;
        }

        public virtual void AddDamage(float damage)
        {
            if (damage > 0)
                this.Damage += damage;
        }

        public virtual void AddHealth(float health)
        {
            if (health > 0)
                this.MaxHealth += health;
        }

        public virtual void Heal(float health)
        {
            
        }

        public virtual void AddBoost(Boost boost)
        {
            this.Damage += boost.damage;
            this.MaxHealth += boost.health;
        }

        public virtual void AddBoosts(Boost[] boosts)
        {
            foreach (var boost in boosts)
            {
                this.AddBoost(boost);
            }
        }

        public virtual float GetDamage()
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (this.CritChance > chance)
            {
                return this.Damage * this.CritMultipler;
            }

            return this.Damage;
        }

        public virtual void ChangeItem()
        {
            var currentItems = WorldModels.Default.Get<Inventory>().CurrentItems;
            
            float damage = 0;
            float speed = 0;
            float health = 0;
            float attackRange = 0;
            float critChance = 0;
            float critMultipler = 0;

            foreach (var item in currentItems)
            {
                if (item.Value != default)
                {
                    damage += item.Value.itemStats.damage;
                    speed += item.Value.itemStats.speed;
                    health += item.Value.itemStats.health;
                    attackRange += item.Value.itemStats.attackRange;

                    if (item.Value.itemType == ItemType.Weapon)
                    {
                        this.AttackTime = item.Value.itemStats.attackSpeed;
                        this.AnimationAttackTime = attackAnimationTime / this.AttackTime;
                        this.FirstAttackTime = firstAttackTime / AnimationAttackTime;
                    }
                }
            }

            Damage = damageWithoutItem + damage;
            Speed = speedWithoutItem + speed;
            MaxHealth = healthWithoutItem + health;
            AttackRange = attackRangeWithouItem + attackRange;
            CritChance = critWithoutItem + critChance;
            CritMultipler = critMultiplerWithoutItem + critMultipler;
        }

        public override string ToString()
        {
            return $"Health: {this.MaxHealth} \nDamage: {this.Damage} \nSpeed: {this.Speed}\n" +
                   $"AttackSpeed: {this.AttackTime}\nAttackRange: {this.AttackRange}\n";
        }
    }
}