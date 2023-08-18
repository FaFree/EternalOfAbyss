using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct ArrowComponent : IComponent
    {
        public float duration;
        public float damage;

        public Vector3 direction;
    }
}