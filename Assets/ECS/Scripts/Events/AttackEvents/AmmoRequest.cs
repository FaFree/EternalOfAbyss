using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct AmmoRequest : IEventData
    {
        public Vector3 spawnPosition;
        public Vector3 direction;
        
        public float damage;

        public bool isPlayer;
        public bool isAutoArrow;
        
        public EntityId entityId;
    }
}