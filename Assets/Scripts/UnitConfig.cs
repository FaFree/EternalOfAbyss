using System;

namespace Scripts
{
    [Serializable]
    public struct UnitConfig
    {
        public string prefab;
        public float speed;
        public int maxHealth;
        public float attackRange;
        public float damage;
        public float attackTime;
        public int coinReward;
        public int xpReward;

        public float critChance;
        public float critMultipler;

        public UnitConfig(string prefab, float speed, int maxHealth, float attackRange, float damage, 
            float attackTime, int coinReward, int xpReward, float critChance, float critMultipler)
        {
            this.prefab = prefab;
            this.speed = speed;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.damage = damage;
            this.attackTime = attackTime;
            this.coinReward = coinReward;
            this.xpReward = xpReward;
            this.critChance = critChance;
            this.critMultipler = critMultipler;
        }
    }
}