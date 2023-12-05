using ECS.Scripts.Events;
using ECS.Scripts.Events.BankEvents;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using Unity.VisualScripting;
using UnityEngine;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts.Components
{
    public class DieUnitSystem : UpdateSystem
    {
        private const float ANIMATION_DIE_TIME = 3.9f;

        private Filter unitDieFilter;

        private Event<DieRequestEvent> dieRequestEvent;
        private Event<DestroyUnitRequestEvent> destroyUnitRequestEvent;

        public override void OnAwake()
        {
            this.dieRequestEvent = this.World.GetEvent<DieRequestEvent>();
            this.destroyUnitRequestEvent = this.World.GetEvent<DestroyUnitRequestEvent>();

            this.unitDieFilter = this.World.Filter.With<DieStateMarker>();

        }

        public override void OnUpdate(float deltaTime)
        {
            if (this.dieRequestEvent.IsPublished)
            {
                foreach (var evt in this.dieRequestEvent.BatchedChanges)
                {
                    if (this.World.TryGetEntity(evt.entityId, out var unit))
                    {
                        ref var healthBar = ref unit.GetComponent<HealthBarComponent>().HealthBarSlider;

                        if (unit.Has<NavMeshAgentComponent>())
                        {
                            ref var unitAgent = ref unit.GetComponent<NavMeshAgentComponent>().agent;
                            unitAgent.isStopped = true;
                        }

                        healthBar.value = 0;
                        unit.GetComponent<HealthComponent>().health = 0;
                        
                        unit.GetComponent<UnitComponent>().stateMachine.SetState<DieMobState>();
                    }
                }
            }

            foreach (var unitEntity in this.unitDieFilter)
            {
                ref var dieMarker = ref unitEntity.GetComponent<DieStateMarker>();

                if (dieMarker.timer > ANIMATION_DIE_TIME)
                {
                    this.destroyUnitRequestEvent.NextFrame(new DestroyUnitRequestEvent
                    {
                        entityId = unitEntity.ID
                    });
                }
                    
                else
                {
                    dieMarker.timer += deltaTime;
                }
            }
        }
    }
}