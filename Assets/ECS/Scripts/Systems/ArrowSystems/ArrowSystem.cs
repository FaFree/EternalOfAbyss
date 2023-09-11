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
        private Filter playerFilter;

        private Event<DieRequestEvent> dieRequest;
        private Event<TextViewRequest> textRequest;

        public override void OnAwake()
        {
            this.arrowFilter = this.World.Filter.With<ArrowComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();

            this.dieRequest = this.World.GetEvent<DieRequestEvent>();
            this.textRequest = this.World.GetEvent<TextViewRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var arrowEntity in arrowFilter)
            {
                ref var arrowComponent = ref arrowEntity.GetComponent<ArrowComponent>();
                ref var arrowTransform = ref arrowEntity.GetComponent<TransformComponent>().transform;
                
                var playerEntity = playerFilter.FirstOrDefault();

                if (playerEntity == default)
                    return;
                
                var boostModel = WorldModels.Default.Get<BoostsModel>();

                arrowComponent.currentDuration += deltaTime;

                if (arrowComponent.currentDuration > arrowComponent.maxDuration)
                {
                    Destroy(arrowTransform.gameObject);
                    this.World.RemoveEntity(arrowEntity);
                    return;
                }
                

                arrowTransform.position += arrowComponent.direction * (deltaTime * arrowComponent.speed);
                
                Ray ray = new Ray(arrowTransform.position, arrowComponent.direction.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1))
                {
                    if (hit.collider.CompareTag("Walls"))
                    {
                        if (arrowComponent.collisionCount > 0 && boostModel.isReboundArrow)
                        {
                            var incomingDirection = arrowComponent.direction;
                            var normal = hit.normal;
                            
                            var newDirection = Vector3.Reflect(incomingDirection, normal);
                            
                            arrowComponent.direction = new Vector3(newDirection.x, 0, newDirection.z).normalized;

                            Quaternion rotation = Quaternion.LookRotation(arrowComponent.direction);

                            arrowTransform.rotation = rotation;
                            arrowComponent.collisionCount--;
                        }
                        else
                        {
                            Destroy(arrowTransform.gameObject);
                            this.World.RemoveEntity(arrowEntity);
                            return;
                        }
                    }
                }

                foreach (var unitEntity in unitFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                    ref var healthComponent = ref unitEntity.GetComponent<HealthComponent>();

                    var sqrDirection = Vector3.SqrMagnitude(arrowTransform.position - unitTransform.position);

                    if (sqrDirection <= 1)
                    {
                        if (healthComponent.health < arrowComponent.damage)
                        {
                            if (healthComponent.health != 0)
                            {
                                ref var zone = ref unitEntity.GetComponent<UnitComponent>().zone;
                                ref var zoneComponent = ref zone.GetComponent<ZoneComponent>();
                                zoneComponent.currentUnitCount--;
                                
                                dieRequest.NextFrame(new DieRequestEvent
                                {
                                    entityId = unitEntity.ID
                                });

                                healthComponent.health = 0;
                            
                                textRequest.NextFrame(new TextViewRequest
                                {
                                    position = unitTransform.position,
                                    text = "-" + arrowComponent.damage
                                });
                                
                                if (!unitEntity.Has<NotAttackMarker>())
                                {
                                    if (arrowComponent.isPassing == false || arrowComponent.passingCount == 0)
                                    {
                                        Destroy(arrowTransform.gameObject);
                                        this.World.RemoveEntity(arrowEntity);
                                        unitEntity.AddComponent<NotAttackMarker>();
                                        return;
                                    }
                                    else
                                    {
                                        arrowComponent.passingCount--;
                                        unitEntity.AddComponent<NotAttackMarker>();
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            healthComponent.health -= arrowComponent.damage;

                            ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();

                            ref var stateMachine = ref unitComponent.stateMachine;
                            ref var navMesh = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;
                            
                            unitComponent.DirectionPosition = Vector3.zero;
                            
                            navMesh.isStopped = true;
                            
                            stateMachine.SetState<AttackMobState>();
                            
                            textRequest.NextFrame(new TextViewRequest
                            {
                                position = unitTransform.position,
                                text = "-" + arrowComponent.damage
                            });
                            
                            if (arrowComponent.isPassing == false || arrowComponent.passingCount == 0)
                            {
                                Destroy(arrowTransform.gameObject);
                                this.World.RemoveEntity(arrowEntity);
                                return;
                            }
                            else
                            {
                                arrowComponent.passingCount--;
                                arrowTransform.position += arrowComponent.direction.normalized;
                            }
                        }
                    }
                }
            }
        }
    }
}