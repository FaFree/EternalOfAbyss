using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.LevelModel
{
    public class Levels : ScriptableObject
    {
        [SerializeField] private List<Level> levels;

        public Dictionary<string, string> levelsMap;

        public void Initialize()
        {
            this.levelsMap = this.levels.ToDictionary(x => x.key, x => x.address);
        }
    }

    [Serializable]
    public struct Level
    {
        public string key;

        public string address;
    }
}