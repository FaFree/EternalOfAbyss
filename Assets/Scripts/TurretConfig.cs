using System;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public struct TurretConfig
    {
        public float damage;
        public float attackSpeed;
        public float cost;

        public float maxLevel;
        public float upgradeCost;

        public GameObject turretObject;

        public void Upgrade()
        {
            
        }
    }
}