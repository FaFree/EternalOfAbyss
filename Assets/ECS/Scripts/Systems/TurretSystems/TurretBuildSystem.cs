using ResourceFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts.Components
{
    public class TurretBuildSystem : UpdateSystem
    {
        private const float MINIMAL_DISTANCE = 5F;

        private Resource coins;
        
        private Filter turretFilter;
        private Filter playerFilter;
        
        public override void OnAwake()
        {
            this.turretFilter = this.World.Filter.With<TurretComponent>().
                With<TransformComponent>();
            
            this.playerFilter = this.World.Filter.
                With<PlayerComponent>().
                With<TransformComponent>();

            coins = Resources.GetResource("Coin");
        }

        public override void OnUpdate(float deltaTime)
        {
            var playerEntity = this.playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;

            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

            foreach (var turretEntity in this.turretFilter)
            {
                ref var turretTransform = ref turretEntity.GetComponent<TransformComponent>().transform;
                ref var turretComponent = ref turretEntity.GetComponent<TurretComponent>();

                var distance = Vector3.SqrMagnitude(playerTransform.position - turretTransform.position);

                if (distance < MINIMAL_DISTANCE)
                {
                    if (coins.IsEnough(turretComponent.config.cost) && !turretEntity.Has<ActiveMarker>())
                    {
                        turretEntity.AddComponent<ActiveMarker>();
                        
                        turretComponent.config.turretObject.SetActive(true);

                        coins.TakeResource(turretComponent.config.cost);
                    }
                    
                    else if (coins.IsEnough(turretComponent.config.upgradeCost) && turretEntity.Has<ActiveMarker>())
                    {
                        turretComponent.config.Upgrade();
                    }
                }
            }
        }
    }
}