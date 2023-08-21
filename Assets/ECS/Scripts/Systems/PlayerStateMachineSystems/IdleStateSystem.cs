using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using State_Machine;
using State_Machine.MobStateMachine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class IdleStateSystem : UpdateSystem
    {
        private Filter playerFilter;
        private Filter mobFilter;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<IdleStateMarker>()
                .With<PlayerComponent>();

            this.mobFilter = this.World.Filter
                .With<UnitComponent>()
                .Without<DieStateMarker>()
                .With<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                var direction = playerEntity.GetComponent<PlayerComponent>().direction;
                ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;

                if (direction != UnityEngine.Vector3.zero)
                {
                    stateMachine.SetState<RunState>();

                    break;
                }

                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>();
                ref var playerComponent = ref playerEntity.GetComponent<PlayerComponent>();

                var nearestEnemy = this.FindNearestEnemy(playerComponent, playerTransform);

                if (nearestEnemy == default)
                {
                    continue;
                }
                
                playerComponent.stateMachine.SetState<AttackState>();
                
                playerEntity.SetComponent(new TargetComponent
                {
                    entityID = nearestEnemy.ID
                });
                
                
            }
        }

        private Entity FindNearestEnemy(PlayerComponent player, TransformComponent playerTransform)
        {
            foreach (var mobEntity in mobFilter)
            {
                var inventory = WorldModels.Default.Get<Inventory>();
                
                ref var mobTransform = ref mobEntity.GetComponent<TransformComponent>().transform;

                var sqrDistance = Vector3.SqrMagnitude(playerTransform.transform.position
                                                       - mobTransform.transform.position);

                var isDie = mobEntity.Has<NotAttackMarker>();
                var isDieAnimation = mobEntity.Has<DieAnimationMarker>();

                if (inventory.CurrentItems[ItemType.Weapon].itemStats.isRangeWeapon && !isDie && !isDieAnimation)
                {
                    Ray ray = new Ray(playerTransform.transform.position,
                        (mobTransform.position - playerTransform.transform.position).normalized);
                    
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, player.UnitPlayerModel.AttackRange))
                    {
                        if (hit.collider.CompareTag("Unit"))
                        {
                            return mobEntity;
                        }
                    }
                }
                else if (player.UnitPlayerModel.CanAttack(sqrDistance) && !isDie && !isDieAnimation)
                {
                    return mobEntity;
                }
            }

            return default;
        }
    }
}