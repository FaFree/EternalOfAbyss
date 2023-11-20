using System;
using Scellecs.Morpeh;
using Scripts;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct BarrierComponent : IComponent
    {
        public BarrierConfig barrierConfig;
    }
}