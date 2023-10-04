using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class AttackUnitStateSystem : UpdateSystem
    {
        private Filter playerFilter;
        private Filter unitFilter;
        
        private float timer;

        private Event<PlayerDieRequestEvent> dieRequestEvent;
        private Event<DamagedEvent> damagedEvent;

        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<TransformComponent>();

            this.unitFilter = this.World.Filter
                .With<UnitComponent>()
                .With<AttackUnitStateMarker>();

            this.dieRequestEvent = this.World.GetEvent<PlayerDieRequestEvent>();
            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in unitFilter)
            {
                ref var attackStateMarker = ref unitEntity.GetComponent<AttackUnitStateMarker>();
                
                attackStateMarker.timer += deltaTime;
                
                ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                ref var unitModel = ref unitComponent.unit;
                ref var stateMachine = ref unitComponent.stateMachine;
                
                var playerEntity = this.playerFilter.FirstOrDefault();

                if (playerEntity == default)
                {
                    attackStateMarker.timer = 0f;
                    unitComponent.stateMachine.SetState<IdleMobState>();
                    return;
                }
                
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

                var sqrDistance = Vector3.SqrMagnitude(playerTransform.transform.position 
                                                       - unitTransform.transform.position);
                
                unitTransform.LookAt(playerTransform);

                if (!unitModel.CanAttack(sqrDistance))
                {
                    attackStateMarker.timer = 0f;
                    stateMachine.SetState<RunMobState>();
                    continue;
                }

                var attackTime = attackStateMarker.isFirstAttack ? unitModel.FirstAttackTime : unitModel.AttackTime;
                
                if (attackStateMarker.timer > attackTime)
                {
                    attackStateMarker.timer = 0f;
                    attackStateMarker.isFirstAttack = false;

                    ref var healthComponent = ref playerEntity.GetComponent<HealthComponent>();
                    var damage = unitModel.Damage; 
                    
                    if (damage >= healthComponent.health)
                    {
                        if (healthComponent.health > 0)
                        {
                            healthComponent.health = 0f;
                            
                            dieRequestEvent.NextFrame(new PlayerDieRequestEvent
                            {
                                entityId = playerEntity.ID
                            });
                        }
                        
                        attackStateMarker.timer = 0f;
                        
                        unitComponent.stateMachine.SetState<IdleMobState>();
                    }
                    else
                    {
                         healthComponent.health -= damage;
                         
                         damagedEvent.NextFrame(new DamagedEvent()
                         {
                             EntityId = playerEntity.ID,
                             Damage =  damage
                         });
                    }
                }

            }
        }
    }
}