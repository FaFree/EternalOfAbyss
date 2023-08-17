using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine.MobStateMachine
{
    public class RunMobState : BaseState
    {
        private Entity entity;
        
        public RunMobState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            entity.AddComponent<RunUnitStateMarker>();
        }

        public override void OnExit()
        {
            entity.RemoveComponent<RunUnitStateMarker>();
        }
    }
}