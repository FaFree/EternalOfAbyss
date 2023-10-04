using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct OnArrowCollisionEnter : IEventData
    {
        public Collision collision;

        public EntityId entityId;
    }
}