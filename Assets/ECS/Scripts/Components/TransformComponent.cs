using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct TransformComponent : IComponent
    {
        public Transform transform;
    }
}