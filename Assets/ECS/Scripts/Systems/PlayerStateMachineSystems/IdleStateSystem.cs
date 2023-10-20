using DefaultNamespace;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using State_Machine;
using State_Machine.MobStateMachine;
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
            foreach (var playerEntity in playerFilter)
            {
                var direction = playerEntity.GetComponent<PlayerComponent>().direction;
                ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;

                if (direction != UnityEngine.Vector3.zero)
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