using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class AttackUnitStateSystem : UpdateSystem
    {
        private Filter unitFilter;
        
        private float timer;

        private Event<BaseDieRequestEvent> baseDieRequestEvent;
        private Event<DestroyBarrierRequest> destroyRequestEvent;
        private Event<DamagedEvent> damagedEvent;
        private Event<NavMeshUpdateRequest> meshUpdateRequest;

        public override void OnAwake()
        {
            this.unitFilter = this.World.Filter
                .With<UnitComponent>()
                .With<AttackUnitStateMarker>();

            this.baseDieRequestEvent = this.World.GetEvent<BaseDieRequestEvent>();
            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
            this.destroyRequestEvent = this.World.GetEvent<DestroyBarrierRequest>();
            this.meshUpdateRequest = this.World.GetEvent<NavMeshUpdateRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in this.unitFilter)
            {
                ref var attackStateMarker = ref unitEntity.GetComponent<AttackUnitStateMarker>();
                
                attackStateMarker.timer += deltaTime;
                
                ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                ref var unitModel = ref unitComponent.unit;
                ref var stateMachine = ref unitComponent.stateMachine;

                var attackedEntityId = unitEntity.GetComponent<TargetMarker>().entityId;

                if (!this.World.TryGetEntity(attackedEntityId, out var attackedEntity))
                {
                    unitEntity.RemoveComponent<TargetMarker>();
                    stateMachine.SetState<RunMobState>();
                    attackStateMarker.timer = 0f;
                    attackStateMarker.isFirstAttack = true;
                    continue;
                }

                var attackTime = attackStateMarker.isFirstAttack ? unitModel.FirstAttackTime : unitModel.AttackTime;
                
                if (attackStateMarker.timer > attackTime)
                {
                    attackStateMarker.timer = 0f;
                    attackStateMarker.isFirstAttack = false;

                    ref var health = ref attackedEntity.GetComponent<HealthComponent>().health;
                    
                    var damage = unitModel.Damage; 
                    
                    if (damage >= health)
                    {
                        if (!attackedEntity.Has<NotAttackMarker>())
                        {
                            health = 0;

                            if (attackedEntity.Has<BaseComponent>())
                            {
                                baseDieRequestEvent.NextFrame(new BaseDieRequestEvent
                                {
                                    entityId = attackedEntityId
                                });
                            }
                            else
                            {
                                destroyRequestEvent.NextFrame(new DestroyBarrierRequest()
                                {
                                    entityId = attackedEntityId
                                });
                                
                                unitEntity.RemoveComponent<TargetMarker>();
                                stateMachine.SetState<RunMobState>();
                                attackStateMarker.timer = 0f;
                                attackStateMarker.isFirstAttack = true;
                                
                                this.meshUpdateRequest.NextFrame(new NavMeshUpdateRequest());
                                
                                continue;
                            }
                        }
                        
                        attackStateMarker.timer = 0f;
                    }
                    else
                    {
                        health -= damage;
                         
                         damagedEvent.NextFrame(new DamagedEvent()
                         {
                             EntityId = attackedEntityId,
                             Damage =  damage,
                             isBaseDamage = true,
                             hitPosition = unitTransform.position
                         });
                    }
                }

            }
        }
    }
}