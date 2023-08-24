using Scripts;
using Scellecs.Morpeh;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitPlayer : BaseUnit
    {
        private float critChance;
        private float critMultipler;
        
        public UnitPlayer(UnitConfig config, float firstAttackTime, float attackAnimationTime,
            float CritChance, float CritMultipler, ref Entity entity) 
            : base(config, firstAttackTime, attackAnimationTime, ref entity)
        {
            this.critChance = CritChance;
            this.critMultipler = CritMultipler;
        }

        public float GetDamage()
        {
            float chance = Random.Range(0f, 1f);

            if (critChance > chance)
            {
                return Damage * critMultipler;
            }

            return Damage;
        }
    }
}