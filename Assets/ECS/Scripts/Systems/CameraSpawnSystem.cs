using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS.Scripts.Components
{
    public class CameraSpawnSystem : UpdateSystem
    {
        private Quaternion CAMERA_ROTATION = Quaternion.Euler(30, 0, 0);
        private Vector3 CAMERA_TRANSFORM = new Vector3(0, 10, -10);
        private Vector3 BUILD_CAMERA_TRANSFORM = new Vector3(10, 60, 15);
        
        private Transform camera;
        private Filter filter;

        private bool isBuildEnded;

        private Event<BuildEndedEvent> buildEndEvent;
        
        public override void OnAwake()
        {
            this.buildEndEvent = this.World.GetEvent<BuildEndedEvent>();
            
            this.filter = this.World.Filter
                .With<PlayerComponent>()
                .With<TransformComponent>();
            var exampleGo = new GameObject("Camera");
            var go = Instantiate(exampleGo);
            Destroy(exampleGo);
            
            go.AddComponent<Camera>();
            go.AddComponent<Physics2DRaycaster>();

            var entity = this.World.CreateEntity();
            entity.SetComponent(new CameraComponent
            {
                transform = go.GetComponent<Transform>()
            });

            camera = go.GetComponent<Transform>();
            camera.rotation = Quaternion.Euler(90, 0,0);
            camera.position = BUILD_CAMERA_TRANSFORM;
            
            WorldModels.Default.Set(camera.GetComponent<Camera>());
        }

        public override void OnUpdate(float deltaTime)
        {
            if (this.buildEndEvent.IsPublished)
            {
                this.isBuildEnded = true;
                this.camera.rotation = CAMERA_ROTATION;
            }

            if (!this.isBuildEnded)
                return;
            
            foreach (var player in filter)
            {
                ref var transform = ref player.GetComponent<TransformComponent>();
                this.camera.position = transform.transform.position + CAMERA_TRANSFORM;
            }
        }
    }
}