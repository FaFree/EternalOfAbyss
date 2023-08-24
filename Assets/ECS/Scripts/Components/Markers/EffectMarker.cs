using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct EffectMarker : IComponent
    {
        public float playTime;
        public float currentTime;
        public GameObject effectObject;
    }
}