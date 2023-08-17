using System;
using Factory;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct ZoneComponent : IComponent
    {
        public int maxUnitCount;
        public int currentUnitCount;
        
        public float radius;

        public bool isSpawned;

        public Transform position;

        public UnitConfig config;
    }
}