using System;
using Scripts;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct ZoneComponent : IComponent
    {
        public float allUnitSpawnTime;

        public int spawnedUnitCount;
        public int maxUnitCount;
        public int currentUnitCount;
        
        public float radius;

        public float timer;

        public bool isEnded;
        public bool isSpawned;

        public WaveProgressConfig waveProgressConfig;
        
        public Transform position;

        public GameObject nextWave;

        public UnitConfig config;

        public GameObject Level;
    }
}