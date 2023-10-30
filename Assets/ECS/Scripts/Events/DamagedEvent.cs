using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct DamagedEvent : IEventData
    {
        public EntityId EntityId;
        public float Damage;

        public bool isBaseDamage;
        public Vector3 hitPosition;
    }
}