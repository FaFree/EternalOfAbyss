using System;
using Unity.AI.Navigation;
using IComponent = Scellecs.Morpeh.IComponent;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct NavMeshSurfaceComponent : IComponent
    {
        public NavMeshSurface navMeshSurface;
    }
}