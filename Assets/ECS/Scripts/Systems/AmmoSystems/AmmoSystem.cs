using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class AmmoSystem : UpdateSystem
    {
        private const float HIT_DISTANCE = 2f;
        
        private Filter unitFilter;
        private Filter arrowFilter;

        private Event<DamageRequest> damageRequest;

        public override void OnAwake()
        {
            this.arrowFilter = this.World.Filter.With<AmmoComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();

            this.damageRequest = this.World.GetEvent<DamageRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var arrowEntity in arrowFilter)
            {
                ref var arrowComponent = ref arrowEntity.GetComponent<AmmoComponent>();
                ref var arrowTransform = ref arrowEntity.GetComponent<TransformComponent>().transform;
                
                ref var ammoTrail = ref arrowEntity.GetComponent<TrailMarker>().trailObject;

                arrowComponent.currentDuration += deltaTime;
                
                ammoTrail.SetActive(true);

                if (arrowComponent.currentDuration > arrowComponent.maxDuration)
                {
                    arrowTransform.gameObject.SetActive(false);
                    arrowEntity.GetComponent<TrailMarker>().trailObject.SetActive(false);
                    this.World.RemoveEntity(arrowEntity);
                    continue;
                }

                if (arrowEntity.Has<AutoArrowMarker>())
                {
                    ref var arrowMarker = ref arrowEntity.GetComponent<AutoArrowMarker>();

                    if (this.World.TryGetEntity(arrowMarker.entityId, out var unitEntity) && !unitEntity.Has<NotAttackMarker>())
                    {
                        ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;

                        var direction = unitTransform.position - arrowTransform.position;

                        arrowTransform.position += direction.normalized * (deltaTime * arrowComponent.speed);
                    }
                    
                    else
                    {
                        arrowTransform.position += arrowTransform.forward * (deltaTime * arrowComponent.speed);
                    }
                }
                else
                {
                    arrowTransform.position += arrowComponent.direction * (deltaTime * arrowComponent.speed);
                }

                foreach (var unitEntity in unitFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;

                    var sqrDirection = Vector3.SqrMagnitude(new Vector3
                        (arrowTransform.position.x, arrowTransform.position.y -1, arrowTransform.position.z) 
                                                            - unitTransform.position);

                    if (sqrDirection <= HIT_DISTANCE)
                    {
                        if (!unitEntity.Has<NotAttackMarker>())
                        {
                            this.damageRequest.NextFrame(new DamageRequest
                            {
                                Damage = arrowComponent.damage,
                                EntityId = unitEntity.ID
                            });

                            if (arrowComponent.isPassing && arrowComponent.passingCount == 0)
                            {
                                arrowTransform.gameObject.SetActive(false);
                                arrowEntity.GetComponent<TrailMarker>().trailObject.SetActive(false);
                                this.World.RemoveEntity(arrowEntity);
                                break;
                            }

                            if (arrowComponent.isPassing && arrowComponent.passingCount != 0)
                            {
                                arrowComponent.passingCount--;
                                arrowTransform.position += arrowComponent.direction.normalized * 2.5f;
                            }

                            else
                            {
                                arrowTransform.gameObject.SetActive(false);
                                arrowEntity.GetComponent<TrailMarker>().trailObject.SetActive(false);
                                this.World.RemoveEntity(arrowEntity);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}