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
        
        private Event<DieRequestEvent> dieRequestEvent;
        private Event<TextViewRequest> textRequest;
        private Event<ArrowRequest> arrowRequest;
        
        private float timer;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<AttackStateMarker>()
                .With<TransformComponent>();
        
            this.dieRequestEvent = this.World.GetEvent<DieRequestEvent>();
            this.textRequest = this.World.GetEvent<TextViewRequest>();
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();
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
                ref var playerModel = ref playerEntity.GetComponent<PlayerComponent>().UnitPlayerModel;
                playerTransform.LookAt(unitTransform);

                if (entity.Has<NotAttackMarker>())
                {
                    this.timer = 0f;
                    
                    stateMachine.SetState<IdleState>();

                    playerEntity.RemoveComponent<TargetComponent>();
                    
                    continue;
                }
                
                if (!this.CanAttackUnit(playerTransform, unitTransform, playerModel))
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

                    ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                    var inventory = WorldModels.Default.Get<Inventory>();

                    if (inventory.CurrentItems[ItemType.Weapon].itemStats.isRangeWeapon)
                    {
                        arrowRequest.NextFrame(new ArrowRequest
                        {
                            direction = (unitTransform.position - playerTransform.position).normalized,
                            spawnPosition = playerTransform.position,
                            damage = playerModel.GetDamage()
                        });
                    }
                    else
                    {
                        var damage = playerModel.GetDamage();
                        
                        if (healthComponent.health > damage)
                        {
                            healthComponent.health -= damage;
                        
                            textRequest.NextFrame(new TextViewRequest()
                            {
                                position = unitTransform.position,
                                text = "-" + damage
                            });
                        }
                        else
                        {
                            ref var zone = ref entity.GetComponent<UnitComponent>().zone;
                            zone.GetComponent<ZoneComponent>().currentUnitCount--;
                        
                            entity.AddComponent<NotAttackMarker>();
                        
                            textRequest.NextFrame(new TextViewRequest()
                            {
                                position = unitTransform.position,
                                text = "-" + playerModel.Damage
                            });
                        
                            dieRequestEvent.NextFrame(new DieRequestEvent
                            {
                                entityId = entity.ID
                            });
                        
                            stateMachine.SetState<IdleState>();
                        }
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