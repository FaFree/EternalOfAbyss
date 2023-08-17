using System.Numerics;
using ECS.Scripts.Components;
using Scellecs.Morpeh;

namespace State_Machine
{
    public class IdleState : BaseState
    {
        private Entity entity;
        public IdleState(StateMachine stateMachine, ref Entity entity) : base(stateMachine)
        {
            this.entity = entity;
        }

        public override void OnEnter()
        {
            this.entity.AddComponent<IdleStateMarker>();
        }

        public override void OnExit()
        {
            this.entity.RemoveComponent<IdleStateMarker>();
        }
        
        public override void OnUpdate()
        {
            
        }
    }
}