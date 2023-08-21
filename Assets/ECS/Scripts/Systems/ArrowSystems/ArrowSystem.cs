using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class ArrowSystem : UpdateSystem
    {
        private Filter unitFilter;
        private Filter arrowFilter;

        private Event<DieRequestEvent> dieRequest;
        private Event<TextViewRequest> textRequest;

        public override void OnAwake()
        {
            this.arrowFilter = this.World.Filter.With<ArrowComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();

            this.dieRequest = this.World.GetEvent<DieRequestEvent>();
            this.textRequest = this.World.GetEvent<TextViewRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var arrowEntity in arrowFilter)
            {
                ref var arrowComponent = ref arrowEntity.GetComponent<ArrowComponent>();
                ref var arrowTransform = ref arrowEntity.GetComponent<TransformComponent>().transform;

                arrowTransform.position += arrowComponent.direction * deltaTime;
                
                Ray ray = new Ray(arrowTransform.position, arrowComponent.direction.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1))
                {
                    if (hit.collider.CompareTag("Walls"))
                    {
                        arrowTransform.DOKill();
                        Destroy(arrowTransform.gameObject);
                        this.World.RemoveEntity(arrowEntity);
                        return;
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
                            
                                unitEntity.AddComponent<NotAttackMarker>();
                            }
                        }
                        else
                        {
                            healthComponent.health -= arrowComponent.damage;
                            
                            textRequest.NextFrame(new TextViewRequest
                            {
                                position = unitTransform.position,
                                text = "-" + arrowComponent.damage
                            });
                        }
                        
                        arrowTransform.DOKill();
                        Destroy(arrowTransform.gameObject);
                        this.World.RemoveEntity(arrowEntity);
                        return;
                    }
                }
            }
        }
    }
}