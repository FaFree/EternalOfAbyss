using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct AmmoComponent : IComponent
    {
        public float currentDuration;
        public float maxDuration;
        public float speed;
        public float damage;

        public int collisionCount;
        public int passingCount;

        public bool isPassing;
        public bool isRebound;

        public Vector3 direction;
    }
}