using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.AttackSystems
{
    public class DamageSystem : UpdateSystem
    {
        private Event<DamageRequest> damageRequest;
        private Event<DamagedEvent> damagedEvent;
        private Event<DieRequestEvent> dieRequest;

        public override void OnAwake()
        {
            this.damageRequest = this.World.GetEvent<DamageRequest>();
            this.dieRequest = this.World.GetEvent<DieRequestEvent>();
            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.damageRequest.IsPublished)
                return;

            foreach (var evt in this.damageRequest.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.EntityId, out var entity))
                {
                    return;
                }

                ref var healthComponent = ref entity.GetComponent<HealthComponent>();

                if (healthComponent.health < evt.Damage)
                {
                    if (healthComponent.health != 0)
                    {
                        dieRequest.NextFrame(new DieRequestEvent
                        {
                            entityId = entity.ID
                        });

                        healthComponent.health = 0;

                        damagedEvent.NextFrame(new DamagedEvent
                        {
                            EntityId = entity.ID,
                            Damage = evt.Damage
                        });

                        entity.AddComponent<NotAttackMarker>();
                    }
                }

                else
                {
                    healthComponent.health -= evt.Damage;

                    ref var unitComponent = ref entity.GetComponent<UnitComponent>();

                    ref var stateMachine = ref unitComponent.stateMachine;
                    
                    ref var navMesh = ref entity.GetComponent<NavMeshAgentComponent>().agent;

                    unitComponent.DirectionPosition = Vector3.zero;

                    navMesh.isStopped = true;

                    stateMachine.SetState<AttackMobState>();

                    damagedEvent.NextFrame(new DamagedEvent
                    {
                        EntityId = entity.ID,
                        Damage = evt.Damage
                    });
                }
            }
        }
    }
}