using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class IdleUnitStateSystem : UpdateSystem
    {
        private Filter playerFilter;
        private Filter mobFilter;
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<TransformComponent>();
            
            this.mobFilter = this.World.Filter
                .With<IdleUnitStateMarker>()
                .With<UnitComponent>();

        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

                foreach (var unitEntity in mobFilter)
                {
                    ref var unitAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var unitModel = ref unitComponent.unit;
                    ref var stateMachine = ref unitComponent.stateMachine;
                    var radius = unitComponent.zone.GetComponent<ZoneComponent>().radius;

                    var sqrDistanceToDirection = Vector3.SqrMagnitude(unitComponent.DirectionPosition
                                                                      - unitTransform.position);

                    if (unitComponent.DirectionPosition == Vector3.zero || sqrDistanceToDirection <= 1)
                    {
                        unitAgent.isStopped = true;
                        
                        var randomX = Random.Range(unitTransform.position.x - radius,
                            unitTransform.position.x + radius);

                        var randomZ = Random.Range(unitTransform.position.z - radius,
                            unitTransform.position.z + radius);

                        var newPosition = new Vector3(randomX, 0, randomZ);
                        unitComponent.DirectionPosition = newPosition;
                    }
                    
                    unitAgent.SetDestination(unitComponent.DirectionPosition);
                    unitAgent.isStopped = false;
                    unitTransform.rotation = Quaternion.LookRotation(unitAgent.velocity.normalized);
                    
                    unitTransform.rotation = Quaternion.identity;
                    
                    var sqrDistance = Vector3.SqrMagnitude(playerTransform.transform.position 
                                                           - unitTransform.transform.position);

                    if (unitModel.CanAttack(sqrDistance))
                    {
                        unitAgent.isStopped = true;
                        stateMachine.SetState<AttackMobState>();
                    }
                }
            }
        }
    }
}