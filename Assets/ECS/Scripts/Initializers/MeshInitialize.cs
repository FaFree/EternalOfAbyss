using DG.Tweening;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class MeshInitialize : Initializer
    {

        private Filter meshFilter;
        public override void OnAwake()
        {
            this.meshFilter = this.World.Filter.With<NavMeshSurfaceComponent>();

            var meshEntity = this.meshFilter.FirstOrDefault();

            ref var mesh = ref meshEntity.GetComponent<NavMeshSurfaceComponent>();

            mesh.navMeshSurface.UpdateNavMesh(mesh.navMeshSurface.navMeshData);
        }
    }
}