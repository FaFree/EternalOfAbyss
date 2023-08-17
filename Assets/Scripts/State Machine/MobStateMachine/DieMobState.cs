using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine.MobStateMachine
{
    public class DieMobState : BaseState
    {
        private Entity entity;
        
        public DieMobState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            this.entity.SetComponent(new DieStateMarker
            {
                timer = 0f
            });
        }

        public override void OnExit()
        {
            this.entity.RemoveComponent<DieStateMarker>();
        }
    }
}