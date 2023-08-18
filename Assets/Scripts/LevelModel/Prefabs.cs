using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.LevelModel
{
    public class Prefabs : ScriptableObject
    {
        [SerializeField] private List<Prefab> prefabs;

        public Dictionary<string, string> prefabMap;

        public void Initialize()
        {
            this.prefabMap = this.prefabs.ToDictionary(x => x.key, x => x.address);
        }
    }

    [Serializable]
    public struct Prefab
    {
        public string key;

        public string address;
    }
}