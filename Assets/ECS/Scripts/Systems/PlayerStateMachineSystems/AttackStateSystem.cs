using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class AttackStateSystem : UpdateSystem
    {   
        private Filter playerFilter;
        
        private Event<RangeAttackRequest> rangeAttackRequest;
        private Event<MeleeAttackRequest> meleeAttackRequest;

        private float timer;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<AttackStateMarker>()
                .With<TransformComponent>();
            
            this.rangeAttackRequest = this.World.GetEvent<RangeAttackRequest>();
            this.meleeAttackRequest = this.World.GetEvent<MeleeAttackRequest>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                this.timer += deltaTime;
                
                ref var stateMachine = ref playerEntity.GetComponent<PlayerComponent>().stateMachine;
                var direction = playerEntity.GetComponent<PlayerComponent>().direction;
                
                if (direction != Vector3.zero)
                {
                    this.timer = 0f;
                    
                    stateMachine.SetState<RunState>();
                    
                    playerEntity.RemoveComponent<TargetComponent>();
                    
                    continue;
                }

                var entityId = playerEntity.GetComponent<TargetComponent>().entityID;
                
                if (!this.World.TryGetEntity(entityId, out var entity))
                {
                    this.timer = 0f;
                    
                    stateMachine.SetState<IdleState>();
                    
                    playerEntity.RemoveComponent<TargetComponent>();

                    continue;
                }
                
                ref var unitTransform = ref entity.GetComponent<TransformComponent>().transform;
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;
                var playerModel = WorldModels.Default.Get<UnitPlayer>();

                if (entity.Has<NotAttackMarker>() || !this.CanAttackUnit(playerTransform, unitTransform, playerModel))
                {
                    this.timer = 0f;
                    
                    stateMachine.SetState<IdleState>();
                    
                    playerEntity.RemoveComponent<TargetComponent>();

                    continue;
                }
                
                ref var attackStateMarker = ref playerEntity.GetComponent<AttackStateMarker>();

                var attackTime = attackStateMarker.isFirstAttack ? playerModel.FirstAttackTime : playerModel.AttackTime;
                
                if (timer > attackTime)
                {
                    attackStateMarker.isFirstAttack = false;
                    
                    this.timer = 0f;

                    var inventory = WorldModels.Default.Get<Inventory>();

                    if (inventory.CurrentItems[ItemType.Weapon].itemStats.isRangeWeapon)
                    {
                        this.rangeAttackRequest.NextFrame(new RangeAttackRequest
                        {
                            entityId = entityId
                        });
                    }
                    else
                    {
                        this.meleeAttackRequest.NextFrame(new MeleeAttackRequest
                        {
                            entityId = entityId
                        });
                    }
                }
            }
        }
        
        private bool CanAttackUnit(Transform playerTransform, Transform unitTransform, UnitPlayer unitPlayerModel)
        {
            var inventory = WorldModels.Default.Get<Inventory>();
            
            var sqrDistance = Vector3.SqrMagnitude(playerTransform.position
                                                   - unitTransform.position);

            if (!inventory.CurrentItems[ItemType.Weapon].itemStats.isRangeWeapon)
            {
                return unitPlayerModel.CanAttack(sqrDistance);
            }

            Ray ray = new Ray(playerTransform.position, (unitTransform.position 
                                                         - playerTransform.position).normalized);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, unitPlayerModel.AttackRange))
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}