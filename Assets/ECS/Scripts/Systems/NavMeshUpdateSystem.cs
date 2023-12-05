using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class NavMeshUpdateSystem : UpdateSystem
    {
        private Filter meshFilter;

        private Event<NavMeshUpdateRequest> updateRequest;

        public override void OnAwake()
        {
            this.meshFilter = this.World.Filter.With<NavMeshSurfaceComponent>();

            this.updateRequest = this.World.GetEvent<NavMeshUpdateRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.updateRequest.IsPublished)
                return;

            foreach (var meshEntity in this.meshFilter)
            {
                ref var mesh = ref meshEntity.GetComponent<NavMeshSurfaceComponent>().navMeshSurface;

                mesh.UpdateNavMesh(mesh.navMeshData);
            }
        }
    }
}