using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class TurretAttackSystem : UpdateSystem
    {
        private Filter unitFilter;
        private Filter turretFilter;

        private Event<ArrowRequest> arrowRequest;
        public override void OnAwake()
        {
            this.unitFilter = this.World.Filter
                .With<UnitComponent>()
                .With<TransformComponent>();

            this.turretFilter = this.World.Filter
                .With<TurretComponent>()
                .With<ActiveMarker>();

            this.arrowRequest = this.World.GetEvent<ArrowRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var turretEntity in this.turretFilter)
            {
                ref var turretTransform = ref turretEntity.GetComponent<TransformComponent>().transform;
                ref var turretComponent = ref turretEntity.GetComponent<TurretComponent>();

                turretComponent.timer += deltaTime;

                var unitEntity = this.GetNearbyUnit(turretTransform.transform);

                if (unitEntity == default)
                {
                    turretComponent.timer = 0f;
                    return;
                }

                if (turretComponent.timer > turretComponent.config.attackSpeed)
                {
                    turretComponent.timer = 0f;
                    
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;

                    var direction = (unitTransform.position - turretTransform.position).normalized;
                
                    this.arrowRequest.NextFrame(new ArrowRequest
                    {
                        damage = turretComponent.config.damage,
                        direction = direction,
                        spawnPosition = turretTransform.position,
                        isPlayer = false,
                        isAutoArrow = true,
                        entityId = unitEntity.ID
                    });

                    Quaternion toRotation = Quaternion.LookRotation(direction);

                    Quaternion fromRotation = turretComponent.config.turretObject.transform.rotation;
                    
                    Quaternion rotationDifference = toRotation * Quaternion.Inverse(fromRotation);
                    
                    turretComponent.config.turretObject.transform.DORotateQuaternion(fromRotation * rotationDifference, 1f);
                }
            }
        }

        private Entity GetNearbyUnit(Transform turretPosition)
        {
            var minDistance = 100000f;
            Entity entity = default;
            
            foreach (var unitEntity in this.unitFilter)
            {
                ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;

                var currentUnitDistance = Vector3.SqrMagnitude(unitTransform.position - turretPosition.position);

                if (currentUnitDistance < minDistance && !unitEntity.Has<NotAttackMarker>())
                {
                    minDistance = currentUnitDistance;
                    entity = unitEntity;
                }
            }

            return entity;
        }
    }
}