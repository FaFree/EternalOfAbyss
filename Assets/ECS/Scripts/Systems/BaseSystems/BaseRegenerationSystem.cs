using System;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class BaseRegenerationSystem : UpdateSystem
    {
        private const float REGENERATION_SPEED = 1f;
        
        private Filter baseFilter;

        private Event<RegenerationEvent> regenerationEvent;

        private float timer;
        
        public override void OnAwake()
        {
            this.baseFilter = this.World.Filter
                .With<BaseComponent>()
                .With<HealthComponent>();

            this.regenerationEvent = this.World.GetEvent<RegenerationEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var baseEntity in this.baseFilter)
            {
                this.timer += deltaTime;

                if (this.timer >= REGENERATION_SPEED)
                {
                    this.timer = 0f;
                    
                    ref var healthComponent = ref baseEntity.GetComponent<HealthComponent>();

                    if (healthComponent.health == 0)
                        return;
                    
                    ref var baseComponent = ref baseEntity.GetComponent<BaseComponent>();

                    var newHealth = healthComponent.health + baseComponent.regeneration;

                    healthComponent.health = Math.Min(newHealth, healthComponent.MaxHealth);
                    
                    regenerationEvent.NextFrame(new RegenerationEvent());
                }
            }
        }
    }
}