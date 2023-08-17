using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class UnitAnimatorSystem : UpdateSystem
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");
        private static readonly int IsDie = Animator.StringToHash("isDie");

        
        private Filter unitFilter;
        public override void OnAwake()
        {
            this.unitFilter = this.World.Filter
                .With<UnitComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in unitFilter)
            {
                ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                ref var animator = ref unitComponent.animator;

                var isRunning = unitEntity.Has<RunUnitStateMarker>();
                var isAttack = unitEntity.Has<AttackUnitStateMarker>();
                var isIdle = unitEntity.Has<IdleUnitStateMarker>();
                var isDie = unitEntity.Has<DieStateMarker>();
                
                animator.SetFloat(AttackSpeed, unitComponent.unit.AnimationAttackTime);
                
                animator.SetBool(IsRunning, isRunning);
                animator.SetBool(IsAttack, isAttack);
                animator.SetBool(IsIdle, isIdle);
                animator.SetBool(IsDie, isDie);
            }
        }
    }
}