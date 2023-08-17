using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class AttackStateSystem : UpdateSystem
    {   
        private Filter playerFilter;
        
        private Event<DieRequestEvent> dieRequestEvent;
        private Event<TextViewRequest> textRequest;
        
        private float timer;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter
                .With<PlayerComponent>()
                .With<AttackStateMarker>()
                .With<TransformComponent>();
        
            this.dieRequestEvent = this.World.GetEvent<DieRequestEvent>();
            this.textRequest = this.World.GetEvent<TextViewRequest>();
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
                
                ref var unitModel = ref entity.GetComponent<UnitComponent>().unit;
                ref var unitTransform = ref entity.GetComponent<TransformComponent>().transform;
                ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;
                ref var playerModel = ref playerEntity.GetComponent<PlayerComponent>().UnitPlayerModel;
                playerTransform.LookAt(unitTransform);

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

                    if (healthComponent.health > playerModel.Damage)
                    {
                        healthComponent.health -= playerModel.Damage;
                        textRequest.NextFrame(new TextViewRequest()
                        {
                            position = unitTransform.position,
                            text = "-" + playerModel.Damage
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
        
        private bool CanAttackUnit(Transform playerTransform, Transform unitTransform, UnitPlayer unitPlayerModel)
        {
            var sqrDistance = Vector3.SqrMagnitude(playerTransform.position
                                                   - unitTransform.position);
            
            return unitPlayerModel.CanAttack(sqrDistance);
        }
    }
}