using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct ArrowComponent : IComponent
    {
        public float speed;
        public float damage;

        public int collisionCount;

        public bool isRebound;

        public Vector3 direction;
    }
}