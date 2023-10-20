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
        private Filter baseFilter;
        private Filter unitFilter;
        public override void OnAwake()
        {
            this.baseFilter = this.World.Filter
                .With<BaseComponent>();
            
            this.unitFilter = this.World.Filter
                .With<RunUnitStateMarker>()
                .With<UnitComponent>()
                .With<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var baseEntity in this.baseFilter)
            {
                ref var baseTransform = ref baseEntity.GetComponent<BaseComponent>().position;

                foreach (var unitEntity in unitFilter)
                {
                    ref var unitTransform = ref unitEntity.GetComponent<TransformComponent>().transform;
                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var unitModel = ref unitComponent.unit;
                    ref var stateMachine = ref unitComponent.stateMachine;
                    ref var unitAgent = ref unitEntity.GetComponent<NavMeshAgentComponent>().agent;

                    unitAgent.speed = unitModel.Speed;
                    
                    if (unitTransform.position.z - baseTransform.localPosition.z <= 1)
                    {
                        unitAgent.isStopped = true;
                        stateMachine.SetState<AttackMobState>();
                        continue;
                    }

                    var direction = new Vector3(0, 0, -1);
                    
                    unitAgent.nextPosition += (direction * deltaTime);
                    unitAgent.isStopped = false;
                    
                    unitTransform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }
}