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
        private const int HIT_DISTANCE = 2;
        
        private Filter baseFilter;
        private Filter unitFilter;
        private Filter barrierFilter;
        
        public override void OnAwake()
        {
            this.baseFilter = this.World.Filter
                .With<BaseComponent>();

            this.barrierFilter = this.World.Filter
                .With<BarrierComponent>()
                .With<HealthComponent>()
                .With<TransformComponent>();

            this.unitFilter = this.World.Filter
                .With<RunUnitStateMarker>()
                .With<UnitComponent>()
                .With<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            var baseEntity = this.baseFilter.FirstOrDefault();
                
            if (baseEntity == default)
                return;
                
            ref var baseTransform = ref baseEntity.GetComponent<BaseComponent>().position;
            
            
                foreach (var unitEntity in this.unitFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var unitModel = ref unitComponent.unit;
                    ref var stateMachine = ref unitComponent.stateMachine;
                    ref var unitAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;
                    
                    foreach (var barrierEntity in this.barrierFilter)
                    {
                        ref var barrierTransform = ref barrierEntity.GetComponent<TransformComponent>().transform;

                        var sqrDistance = Vector3.SqrMagnitude(barrierTransform.position - unitTransform.position);
                        
                        if (sqrDistance <= Math.Pow(HIT_DISTANCE, 2))
                        {
                            unitAgent.isStopped = true;
                            stateMachine.SetState<AttackMobState>();
                            unitEntity.SetComponent(new TargetMarker
                            {
                                entityId = barrierEntity.ID
                            });
                            return;
                        }
                    }

                    unitAgent.speed = unitModel.Speed;

                    if (Math.Pow(unitTransform.position.z - baseTransform.position.z, 2) <= Math.Pow(HIT_DISTANCE, 2)) 
                    {
                        unitAgent.isStopped = true;
                        stateMachine.SetState<AttackMobState>();
                        
                        unitEntity.SetComponent(new TargetMarker
                        {
                            entityId = baseEntity.ID
                        });
                        
                        continue;
                    }

                    var direction = new Vector3(unitTransform.position.x, baseTransform.position.y, baseTransform.position.z) - unitTransform.position;
                    
                    unitAgent.nextPosition += (direction.normalized * (deltaTime * unitModel.Speed));
                    unitAgent.isStopped = false;
                    
                    unitTransform.rotation = Quaternion.LookRotation(direction);
                } 
        }
    }
}