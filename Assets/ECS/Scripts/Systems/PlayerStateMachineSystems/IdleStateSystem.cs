using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class IdleStateSystem : UpdateSystem
    {
        private Filter playerFilter;

        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<IdleStateMarker>()
                .With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in this.playerFilter)
            {
                var direction = playerEntity.GetComponent<PlayerComponent>().direction;
                ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;

                if (direction != Vector3.zero)
                {
                    stateMachine.SetState<RunState>();

                    break;
                }

                ref var playerComponent = ref playerEntity.GetComponent<PlayerComponent>();

                playerComponent.stateMachine.SetState<AttackState>();
            }
        }
    }
}