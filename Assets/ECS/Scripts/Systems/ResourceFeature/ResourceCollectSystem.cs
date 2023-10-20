using System;
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
        private Filter resourceFilter;
        private Filter playerFilter;

        private float speed = 6f;

        private Event<OnResourceChanged> OnResourceChanged;
        public override void OnAwake()
        {
            this.resourceFilter = this.World.Filter.With<ResourceComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();

            this.OnResourceChanged = this.World.GetEvent<OnResourceChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            var playerEntity = playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;

            var playerTransform = playerEntity.GetComponent<TransformComponent>().transform;

            foreach (var resourceEntity in resourceFilter)
            {
                ref var resourceComponent = ref resourceEntity.GetComponent<ResourceComponent>();
                ref var resourceTransform = ref resourceComponent.transform;

                var direction = playerTransform.position - resourceTransform.position;

                var sqrDistance = Vector3.SqrMagnitude(direction);

                resourceTransform.position += direction * (deltaTime * speed);

                if (sqrDistance < 0.7f)
                {
                    var resource = Resources.GetResource(resourceComponent.resourceType);

                    resource.AddResource(resourceComponent.reward);

                    Resources.SaveResources();

                    OnResourceChanged.NextFrame(new OnResourceChanged());

                    resourceTransform.DOKill();

                    resourceTransform.gameObject.SetActive(false);
                    this.World.RemoveEntity(resourceEntity);
                }
            }
        }
    }
}