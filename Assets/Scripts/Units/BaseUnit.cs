using System;
using System.Collections.Generic;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scripts.InventoryFeature;

namespace DefaultNamespace
{
    public abstract class BaseUnit
    {
        private float healthWithoutItem;
        private float damageWithoutItem;
        private float attackRangeWithouItem;
        private float speedWithoutItem;
        
        private float attackAnimationTime;
        private float firstAttackTime;
        
        protected Entity entity;
        public float MaxHealth { get; private set; }

        public float AttackRange { get; private set; }
        
        public float Damage { get; private set; }
        
        public float Speed { get; private set; }
        
        public float AttackTime { get; private set; }
        public float FirstAttackTime { get; private set; }
        
        public float AnimationAttackTime { get; private set; }
        
        public BaseUnit(UnitConfig config, float firstAttackTime, float attackAnimationTime, ref Entity entity)
        {
            this.MaxHealth = config.maxHealth;
            this.Damage = config.damage;
            this.AttackRange = config.attackRange;
            this.Speed = config.speed;
            this.AttackTime = config.attackTime;
            this.AnimationAttackTime = attackAnimationTime / this.AttackTime;
            this.FirstAttackTime = firstAttackTime / AnimationAttackTime;

            this.damageWithoutItem = Damage;
            this.healthWithoutItem = MaxHealth;
            this.speedWithoutItem = Speed;
            this.attackRangeWithouItem = AttackRange;

            this.attackAnimationTime = attackAnimationTime;
            this.firstAttackTime = firstAttackTime;
            
            this.entity = entity;
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

        public virtual void ChangeItem(Dictionary<ItemType, Item> currentItems)
        {
            float damage = 0;
            float speed = 0;
            float health = 0;
            float attackRange = 0;

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
        }
    }
}