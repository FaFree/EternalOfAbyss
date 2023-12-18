using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class BarrierStatConfig : ScriptableObject
    {
        public float health;
        public string prefab;
        public string ghostPrefab;
    }
}