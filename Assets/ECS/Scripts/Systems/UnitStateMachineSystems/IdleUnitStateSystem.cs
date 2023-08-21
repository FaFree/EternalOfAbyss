using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using State_Machine.MobStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Scripts.Components.MobStateMachineSystems
{
    public class IdleUnitStateSystem : UpdateSystem
    {
        private bool isCorrected;
        private NavMeshPath path;
        
        private Filter playerFilter;
        private Filter mobFilter;
        public override void OnAwake()
        {
            path = new NavMeshPath();
            
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
                    ref var zoneComponent = ref unitComponent.zone.GetComponent<ZoneComponent>();
                    
                    var radius = zoneComponent.radius;

                    unitAgent.speed = 0.5f;
                    
                    var sqrDistanceToDirection = Vector3.SqrMagnitude(unitComponent.DirectionPosition
                                                                      - unitTransform.position);

                    if (unitComponent.DirectionPosition == Vector3.zero || sqrDistanceToDirection <= 1.5)
                    {
                        
                        isCorrected = false;
                        unitAgent.isStopped = true;
                        
                        var randomX = Random.Range(zoneComponent.position.position.x - radius,
                            zoneComponent.position.position.x + radius);

                        var randomZ = Random.Range(zoneComponent.position.position.z - radius,
                            zoneComponent.position.position.z + radius);

                        var newPosition = new Vector3(randomX, 0, randomZ);
                        
                        unitAgent.CalculatePath(newPosition, path);

                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            unitComponent.DirectionPosition = newPosition;
                            unitAgent.SetDestination(unitComponent.DirectionPosition);
                            unitAgent.isStopped = false;
                            isCorrected = true;
                        }
                    }
                    
                    unitTransform.rotation = Quaternion.LookRotation(unitAgent.velocity.normalized);
                    
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