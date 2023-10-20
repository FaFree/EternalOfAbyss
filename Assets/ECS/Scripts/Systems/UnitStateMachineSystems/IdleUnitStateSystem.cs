using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class IdleUnitStateSystem : UpdateSystem
    {
        private Filter baseFilter;
        private Filter mobFilter;

        public override void OnAwake()
        {
            this.mobFilter = this.World.Filter
                .With<IdleUnitStateMarker>()
                .With<UnitComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in mobFilter)
            {
                {

                    ref var unitAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;
                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var unitModel = ref unitComponent.unit;
                    ref var stateMachine = ref unitComponent.stateMachine;

                    unitAgent.speed = unitModel.Speed;

                    if (!unitAgent.hasPath && unitAgent.isOnNavMesh)
                    {
                        stateMachine.SetState<RunMobState>();
                    }

                }
            }
        }
    }
}