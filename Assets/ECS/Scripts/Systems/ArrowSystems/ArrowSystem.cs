using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class ArrowSystem : UpdateSystem
    {
        private Filter unitFilter;
        private Filter arrowFilter;

        private Event<DamageRequest> damageRequest;
        private Event<OnArrowCollisionEnter> onArrowCollisionEnter;

        public override void OnAwake()
        {
            this.arrowFilter = this.World.Filter.With<ArrowComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();

            this.damageRequest = this.World.GetEvent<DamageRequest>();
            this.onArrowCollisionEnter = this.World.GetEvent<OnArrowCollisionEnter>();

        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var arrowEntity in arrowFilter)
            {
                ref var arrowComponent = ref arrowEntity.GetComponent<ArrowComponent>();
                ref var arrowTransform = ref arrowEntity.GetComponent<TransformComponent>().transform;

                arrowComponent.currentDuration += deltaTime;

                if (arrowComponent.currentDuration > arrowComponent.maxDuration)
                {
                    arrowTransform.gameObject.SetActive(false);
                    this.World.RemoveEntity(arrowEntity);
                    continue;
                }

                arrowTransform.position += arrowComponent.direction * (deltaTime * arrowComponent.speed);

                foreach (var unitEntity in unitFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;

                    var sqrDirection = Vector3.SqrMagnitude(arrowTransform.position - unitTransform.position);

                    if (sqrDirection <= 1)
                    {
                        if (!unitEntity.Has<NotAttackMarker>())
                        {
                            this.damageRequest.NextFrame(new DamageRequest
                            {
                                Damage = arrowComponent.damage,
                                EntityId = unitEntity.ID
                            });

                            if (arrowComponent.isPassing == true && arrowComponent.passingCount == 0)
                            {
                                arrowTransform.gameObject.SetActive(false);
                                this.World.RemoveEntity(arrowEntity);
                                break;
                            }

                            else if (arrowComponent.isPassing == true && arrowComponent.passingCount != 0)
                            {
                                arrowComponent.passingCount--;
                                arrowTransform.position += arrowComponent.direction;
                            }

                            else
                            {
                                arrowTransform.gameObject.SetActive(false);
                                this.World.RemoveEntity(arrowEntity);
                                break;
                            }
                        }
                    }
                }
            }

            this.CheckCollision();
        }

        private void CheckCollision()
        {
            if (!this.onArrowCollisionEnter.IsPublished)
            {
                return;
            }

            var boostModel = WorldModels.Default.Get<BoostsModel>();

            foreach (var evt in this.onArrowCollisionEnter.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.entityId, out var arrowEntity))
                {
                    continue;
                }

            }
        }
    }
}