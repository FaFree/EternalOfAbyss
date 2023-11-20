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

        public Transform turretTransform;

        public float buildTime;

        public GameObject turretObject;

        public Slider progressBar;
        public GameObject progressBarObj;
    }
}