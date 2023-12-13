using DefaultNamespace;
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
        private static readonly int IsGun = Animator.StringToHash("isGun");
        
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
            foreach (var playerEntity in this.playerFilter)
            {
                ref var player = ref playerEntity.GetComponent<PlayerComponent>();
                var animator = player.animator;
                
                var isRunning = playerEntity.Has<RunStateMarker>();
                var isIdle = playerEntity.Has<IdleStateMarker>();
                var isAttack = playerEntity.Has<AttackStateMarker>();

                bool isGun = inventory.CurrentItems[ItemType.Weapon].isGun;

                animator.SetFloat(AttackSpeed, WorldModels.Default.Get<Player>().AnimationAttackTime);
                
                animator.SetBool(IsRunning, isRunning);
                animator.SetBool(IsIdle, isIdle);
                animator.SetBool(IsAttack, isAttack);
                animator.SetBool(IsGun, isGun);
            }
        }
    }
}