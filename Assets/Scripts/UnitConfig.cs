using System;

namespace Factory
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

        public UnitConfig(string prefab, float speed, int maxHealth, float attackRange, float damage, float attackTime, int coinReward, int xpReward)
        {
            this.prefab = prefab;
            this.speed = speed;
            this.maxHealth = maxHealth;
            this.attackRange = attackRange;
            this.damage = damage;
            this.attackTime = attackTime;
            this.coinReward = coinReward;
            this.xpReward = xpReward;
        }
    }
}