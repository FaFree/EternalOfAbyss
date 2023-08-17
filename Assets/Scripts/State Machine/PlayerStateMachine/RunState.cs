using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine
{
    public class RunState : BaseState
    {
        private Entity entity;
        
        public RunState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            this.entity.AddComponent<RunStateMarker>();
        }

        public override void OnExit()
        {
            this.entity.RemoveComponent<RunStateMarker>();
        }
    }
}