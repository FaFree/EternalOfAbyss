using System;
using Scellecs.Morpeh;
using Scripts;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct TurretComponent : IComponent
    {
        public float timer;
        
        public TurretConfig config;
    }
}