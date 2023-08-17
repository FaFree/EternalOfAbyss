using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class RunStateSystem : UpdateSystem
    {
        private Filter filter;
        public override void OnAwake()
        {
            this.filter = this.World.Filter
                .With<PlayerComponent>()
                .With<RunStateMarker>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in filter)
            {
                ref PlayerComponent playerComponent = ref playerEntity.GetComponent<PlayerComponent>();
                ref StateMachine stateMachine = ref playerComponent.stateMachine;
                ref var navMeshAgent = ref playerEntity.GetComponent<NavMeshAgentComponent>().agent;
                var playerSpeed = playerComponent.speed;
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

                var directionV3 = playerComponent.direction;
                
                if (directionV3 == Vector3.zero)
                    stateMachine.SetState<IdleState>();

                navMeshAgent.nextPosition += (directionV3 * (playerSpeed * deltaTime));
                playerTransform.transform.rotation = Quaternion.LookRotation(directionV3);
            }
        }
    }
}