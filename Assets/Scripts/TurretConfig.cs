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
        
        public float buildTime;

        public GameObject turretObject;
    }
}