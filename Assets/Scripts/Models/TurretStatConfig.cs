using System;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class TurretStatConfig : ScriptableObject
    {
        public float damage;
        public string ammoKey;
        public string prefab;
        public string ghostPrefab;
    }
}