using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct ArrowRequest : IEventData
    {
        public Vector3 spawnPosition;
        public Vector3 direction;
        public float damage;
    }
}