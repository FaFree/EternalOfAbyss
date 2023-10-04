using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components.AttackSystems
{
    public class MeleeAttackSystem : UpdateSystem
    {
        private Event<MeleeAttackRequest> meleeAttackRequest;
        private Event<DamageRequest> damageRequest;

        private Filter playerFilter;
        public override void OnAwake()
        {
            this.meleeAttackRequest = this.World.GetEvent<MeleeAttackRequest>();
            this.damageRequest = this.World.GetEvent<DamageRequest>();
            
            this.playerFilter = this.World.Filter.With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.meleeAttackRequest.IsPublished)
                return;

            var playerEntity = playerFilter.FirstOrDefault();
            
            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;
            ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;
            var playerModel = WorldModels.Default.Get<UnitPlayer>();
            
            foreach (var evt in this.meleeAttackRequest.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.entityId, out var entity))
                {
                    stateMachine.SetState<IdleState>();
                    continue;
                }

                ref var unitTransform = ref entity.GetComponent<TransformComponent>().transform;
                
                
                playerTransform.LookAt(unitTransform);
                        
                var damage = playerModel.GetDamage();
                        
                this.damageRequest.NextFrame(new DamageRequest
                {
                    Damage = damage,
                    EntityId = entity.ID
                });
            }
        }
    }
}