using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using UnityEngine;
using State_Machine;

namespace ECS.Scripts.Components.AttackSystems
{
    public class RangeAttackSystem : UpdateSystem
    {
        private Event<RangeAttackRequest> rangeAttackRequest;
        private Event<ArrowRequest> arrowRequest;

        private Filter playerFilter;
        public override void OnAwake()
        {
            this.rangeAttackRequest = this.World.GetEvent<RangeAttackRequest>();
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();

            this.playerFilter = this.World.Filter.With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.rangeAttackRequest.IsPublished)
                return;

            var playerEntity = playerFilter.FirstOrDefault();
            
            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;
            ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;
            
            var playerModel = WorldModels.Default.Get<UnitPlayer>();
            
            foreach (var evt in this.rangeAttackRequest.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.entityId, out var entity))
                {
                    stateMachine.SetState<IdleState>();
                    continue;
                }

                ref var unitTransform = ref entity.GetComponent<TransformComponent>().transform;

                var moveDirection = (unitTransform.position - playerTransform.position).normalized;

                Quaternion rotate = Quaternion.LookRotation(moveDirection);

                rotate = rotate * Quaternion.Euler(0, 90, 0);

                playerTransform.rotation = rotate;
                        
                arrowRequest.NextFrame(new ArrowRequest
                {
                    direction = moveDirection,
                    spawnPosition = playerTransform.position,
                    damage = playerModel.GetDamage()
                });
            }
        }
    }
}