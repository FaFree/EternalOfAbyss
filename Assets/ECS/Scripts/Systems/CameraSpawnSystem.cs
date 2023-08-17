using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class CameraSpawnSystem : UpdateSystem
    {
        private Transform camera;
        private Filter filter;
        public override void OnAwake()
        {
            this.filter = this.World.Filter
                .With<PlayerComponent>()
                .With<TransformComponent>();
            var exampleGo = new GameObject("Camera");
            var go = Instantiate(exampleGo);
            Destroy(exampleGo);
            
            go.AddComponent<Camera>();

            var entity = this.World.CreateEntity();
            entity.SetComponent(new CameraComponent
            {
                transform = go.GetComponent<Transform>()
            });

            camera = go.GetComponent<Transform>();
            camera.rotation = Quaternion.Euler(40, 0,0);
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var player in filter)
            {
                ref var transform = ref player.GetComponent<TransformComponent>();
                this.camera.position = transform.transform.position + new Vector3(0, 10, -10);
            }
        }
    }
}