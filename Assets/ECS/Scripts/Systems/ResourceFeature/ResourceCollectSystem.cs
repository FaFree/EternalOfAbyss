using DG.Tweening;
using ECS.Scripts.Components;
using ECS.Scripts.Events.BankEvents;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts
{
    public class ResourceCollectSystem : UpdateSystem
    {
        private const float COIN_SPEED = 6f;

        private Filter resourceFilter;
        private Filter playerFilter;
        
        private Event<OnResourceChanged> onResourceChanged;
        public override void OnAwake()
        {
            this.resourceFilter = this.World.Filter.With<ResourceComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();

            this.onResourceChanged = this.World.GetEvent<OnResourceChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            var playerEntity = this.playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;

            var playerTransform = playerEntity.GetComponent<TransformComponent>().transform;

            foreach (var resourceEntity in this.resourceFilter)
            {
                ref var resourceComponent = ref resourceEntity.GetComponent<ResourceComponent>();
                ref var resourceTransform = ref resourceComponent.transform;

                var direction = playerTransform.position - resourceTransform.position;

                var sqrDistance = Vector3.SqrMagnitude(direction);

                resourceTransform.position += direction * (deltaTime * COIN_SPEED);

                if (sqrDistance < 0.7f)
                {
                    var resource = Resources.GetResource(resourceComponent.resourceType);

                    resource.AddResource(resourceComponent.reward);

                    Resources.SaveResources();

                    onResourceChanged.NextFrame(new OnResourceChanged());

                    resourceTransform.DOKill();

                    resourceTransform.gameObject.SetActive(false);
                    
                    this.World.RemoveEntity(resourceEntity);
                }
            }
        }
    }
}