using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class RunUnitStateSystem : UpdateSystem
    {
        private Filter playerFilter;
        private Filter mobFilter;
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<TransformComponent>();

            this.mobFilter = this.World.Filter
                .With<RunUnitStateMarker>()
                .With<UnitComponent>()
                .With<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

                foreach (var unitEntity in mobFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var unitModel = ref unitComponent.unit;
                    ref var stateMachine = ref unitComponent.stateMachine;
                    ref var unitAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;

                    var sqrDistance = Vector3.SqrMagnitude(playerTransform.transform.position
                                                           - unitTransform.transform.position);

                    var sqrDistanceToSpawn = Vector3.SqrMagnitude(unitTransform.position
                                                                  - unitComponent.spawnPosition);

                    if (sqrDistanceToSpawn > Math.Pow(unitComponent.zone.GetComponent<ZoneComponent>().radius, 2))
                    {
                        unitAgent.isStopped = true;
                        stateMachine.SetState<ComeBackMobState>();
                    }
                    else if (unitModel.CanAttack(sqrDistance))
                    {
                        unitAgent.isStopped = true;
                        stateMachine.SetState<AttackMobState>();
                        continue;
                    }
                    
                    var direction = playerTransform.position - unitTransform.position;
                    direction.y = 0f;

                    if (direction.sqrMagnitude > 0.01f)
                    {
                        unitAgent.SetDestination(playerTransform.position);
                        unitAgent.isStopped = false;
                        unitTransform.rotation = Quaternion.LookRotation(unitAgent.velocity.normalized);
                    }
                }
            }
        }
    }
}