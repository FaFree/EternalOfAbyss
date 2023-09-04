using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class PlayerAnimatorSystem : UpdateSystem
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        private static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");
        private static readonly int IsRange = Animator.StringToHash("isRange");
        
        private Filter playerFilter;
        private Inventory inventory;

        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>();

            inventory = WorldModels.Default.Get<Inventory>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref playerEntity.GetComponent<PlayerComponent>();
                var animator = player.animator;
                
                var isRunning = playerEntity.Has<RunStateMarker>();
                var isIdle = playerEntity.Has<IdleStateMarker>();
                var isAttack = playerEntity.Has<AttackStateMarker>();

                bool isRange = false;

                if (inventory.CurrentItems[ItemType.Weapon] != default)
                    isRange = inventory.CurrentItems[ItemType.Weapon].itemStats.isRangeWeapon;
                
                animator.SetFloat(AttackSpeed, player.UnitPlayerModel.AnimationAttackTime);
                
                animator.SetBool(IsRunning, isRunning);
                animator.SetBool(IsIdle, isIdle);
                animator.SetBool(IsAttack, isAttack);
                animator.SetBool(IsRange, isRange);
            }
        }
    }
}