using System;
using UnityEngine;
using UnityEngine.UI;

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

        public float buildTime;

        public GameObject turretObject;

        public Slider progressBar;
        public GameObject progressBarObj;

        public void Upgrade()
        {
            
        }
    }
}