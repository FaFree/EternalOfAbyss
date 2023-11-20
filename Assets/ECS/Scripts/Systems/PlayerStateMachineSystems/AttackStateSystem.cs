using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class AttackStateSystem : UpdateSystem
    {   
        private Filter playerFilter;
        
        private Event<RangeAttackRequest> rangeAttackRequest;

        private float timer;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<AttackStateMarker>()
                .With<TransformComponent>();
            
            this.rangeAttackRequest = this.World.GetEvent<RangeAttackRequest>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                this.timer += deltaTime;

                ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;
                var direction = playerEntity.GetComponent<PlayerComponent>().direction;

                if (direction != Vector3.zero)
                {
                    this.timer = 0f;

                    stateMachine.SetState<RunState>();

                    continue;
                }

                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

                var playerModel = WorldModels.Default.Get<Player>();

                ref var attackStateMarker = ref playerEntity.GetComponent<AttackStateMarker>();

                var attackTime = attackStateMarker.isFirstAttack ? playerModel.FirstAttackTime : playerModel.AttackTime;

                if (timer > attackTime)
                {
                    attackStateMarker.isFirstAttack = false;

                    this.timer = 0f;
                    
                    this.rangeAttackRequest.NextFrame(new RangeAttackRequest());
                }
            }
        }
    }
}