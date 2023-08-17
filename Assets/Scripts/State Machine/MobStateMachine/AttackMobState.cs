using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine.MobStateMachine
{
    public class AttackMobState : BaseState
    {
        private Entity entity;
        
        public AttackMobState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            entity.SetComponent(new AttackUnitStateMarker
            {
                isFirstAttack = true,
                timer = 0f
            });
        }

        public override void OnExit()
        {
            entity.RemoveComponent<AttackUnitStateMarker>();
        }
    }
}