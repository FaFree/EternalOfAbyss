using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct OnDamage : IEventData
    {
        public Vector3 unitPosition;
        public float damage;
    }
}