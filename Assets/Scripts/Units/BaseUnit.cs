using System;
using System.Collections.Generic;
using Scripts;
using Scripts.InventoryFeature;

namespace DefaultNamespace
{
    public abstract class BaseUnit
    {
        public float CritChance { get; private set; }
        public float CritMultipler { get; private set; }
        
        public float MaxHealth { get; set; }

        public float AttackRange { get; private set; }
        
        public float Damage { get; set; }
        
        public float Speed { get; set; }
        
        public float AttackTime { get; set; }
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
        
        public virtual float GetDamage()
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (this.CritChance > chance)
            {
                return this.Damage * this.CritMultipler;
            }

            return this.Damage;
        }

        public override string ToString()
        {
            return $"Damage: {this.Damage} \nSpeed: {this.Speed}\n" +
                   $"AttackSpeed: {this.AttackTime}\nCrit: {this.CritChance}\n";
        }
    }
}