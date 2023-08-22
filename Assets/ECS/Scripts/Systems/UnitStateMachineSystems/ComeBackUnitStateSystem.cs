using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class ComeBackUnitStateSystem : UpdateSystem
    {
        private Filter unitFilter;
        
        public override void OnAwake()
        {
            this.unitFilter = this.World.Filter
                .With<UnitComponent>()
                .With<ComeBackUnitStateMarker>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in unitFilter)
            {
                ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                ref var unitModel = ref unitComponent.unit;
                ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                var unitSpawnPosition = unitComponent.spawnPosition;
                ref var navMeshAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;

                navMeshAgent.speed = 3.5f;
                
                var direction = unitSpawnPosition - unitTransform.position;
                direction.y = 0;

                if (direction.sqrMagnitude > 0.1f)
                {
                    if (!navMeshAgent.hasPath)
                    {
                        navMeshAgent.SetDestination(unitSpawnPosition);
                    }
                    else
                    {
                        navMeshAgent.ResetPath();
                        navMeshAgent.SetDestination(unitSpawnPosition);
                    }
                    
                    navMeshAgent.isStopped = false;
                    unitTransform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
                }
                else
                {
                    navMeshAgent.isStopped = true;
                    unitEntity.GetComponent<HealthComponent>().health = unitModel.MaxHealth;
                    unitComponent.stateMachine.SetState<IdleMobState>();
                }
            }
        }
    }
}