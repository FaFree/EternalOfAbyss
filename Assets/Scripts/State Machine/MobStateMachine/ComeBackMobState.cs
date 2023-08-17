using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine.MobStateMachine
{
    public class ComeBackMobState : BaseState
    {
        private Entity entity;
        
        public ComeBackMobState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            entity.AddComponent<ComeBackUnitStateMarker>();
        }

        public override void OnExit()
        {
            entity.RemoveComponent<ComeBackUnitStateMarker>();
        }
    }
}