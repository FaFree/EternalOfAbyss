using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine.MobStateMachine
{
    public class IdleMobState : BaseState
    {
        private Entity entity;
        
        public IdleMobState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            entity.AddComponent<IdleUnitStateMarker>();
        }

        public override void OnExit()
        {
            entity.RemoveComponent<IdleUnitStateMarker>();
        }
        
    }
}