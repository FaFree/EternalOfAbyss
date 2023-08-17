using System;
using Scripts;
using Scellecs.Morpeh;

namespace DefaultNamespace
{
    public abstract class BaseUnit
    {
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
    }
}