using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine
{
    public class AttackState : BaseState
    {
        private Filter filter;
        private Entity entity;
        public AttackState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            this.entity.SetComponent(new AttackStateMarker
            {
                isFirstAttack = true
            });
        }

        public override void OnExit()
        { 
            this.entity.RemoveComponent<AttackStateMarker>();
        }
    }
}