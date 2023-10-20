using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class AttackUnitStateSystem : UpdateSystem
    {
        private Filter baseFilter;
        private Filter unitFilter;
        
        private float timer;

        private Event<BaseDieRequestEvent> dieRequestEvent;
        private Event<DamagedEvent> damagedEvent;

        public override void OnAwake()
        {
            this.baseFilter = this.World.Filter
                .With<BaseComponent>()
                .With<HealthComponent>();

            this.unitFilter = this.World.Filter
                .With<UnitComponent>()
                .With<AttackUnitStateMarker>();

            this.dieRequestEvent = this.World.GetEvent<BaseDieRequestEvent>();
            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
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
                
                var baseEntity = this.baseFilter.FirstOrDefault();

                if (baseEntity == default)
                {
                    attackStateMarker.timer = 0f;
                    return;
                }
                
                ref var baseTransform = ref baseEntity.GetComponent<BaseComponent>().position;

                var attackTime = attackStateMarker.isFirstAttack ? unitModel.FirstAttackTime : unitModel.AttackTime;
                
                if (attackStateMarker.timer > attackTime)
                {
                    attackStateMarker.timer = 0f;
                    attackStateMarker.isFirstAttack = false;

                    ref var baseHealth = ref baseEntity.GetComponent<HealthComponent>().health;
                    
                    var damage = unitModel.Damage; 
                    
                    if (damage >= baseHealth)
                    {
                        if (baseHealth > 0)
                        {
                            baseHealth = 0;
                            
                            dieRequestEvent.NextFrame(new BaseDieRequestEvent
                            {
                                entityId = baseEntity.ID
                            });
                        }
                        
                        attackStateMarker.timer = 0f;
                    }
                    else
                    {
                        baseHealth -= (int) damage;
                         
                         damagedEvent.NextFrame(new DamagedEvent()
                         {
                             EntityId = baseEntity.ID,
                             Damage =  damage
                         });
                    }
                }

            }
        }
    }
}