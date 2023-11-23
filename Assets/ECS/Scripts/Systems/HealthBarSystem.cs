using DefaultNamespace;
using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class HealthBarSystem : UpdateSystem
    {
        private Filter entityFilter;

        private Quaternion angle = Quaternion.Euler(30, 0, 0);

        private const float ANIMATION_DURATION = 0.2f;

        private Event<DamagedEvent> damagedEvent;
        private Event<RegenerationEvent> regenerationEvent;

        public override void OnAwake()
        {
            this.entityFilter = this.World.Filter
                .With<HealthComponent>()
                .With<HealthBarComponent>();

            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
            this.regenerationEvent = this.World.GetEvent<RegenerationEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in this.entityFilter)
            {
                ref var healthBarComponent = ref entity.GetComponent<HealthBarComponent>();
                ref var canvasTransform = ref healthBarComponent.CanvasTransform;
                
                canvasTransform.rotation = angle;
                
                ref var healthBar = ref healthBarComponent.HealthBarSlider;
                ref var health = ref entity.GetComponent<HealthComponent>().health;
                ref var maxHealth = ref entity.GetComponent<HealthComponent>().MaxHealth;

                if (this.damagedEvent.IsPublished)
                {
                    healthBar.DOKill();
                
                    healthBar
                        .DOValue(GetHealthOnPercent(health, maxHealth), ANIMATION_DURATION)
                        .SetAutoKill(true);

                    if (entity.Has<BaseComponent>())
                    {
                        ref var baseComponent = ref entity.GetComponent<BaseComponent>();

                        baseComponent.healthView.text = $"{health}/{maxHealth}";
                    }
                }

                if (this.regenerationEvent.IsPublished)
                {
                    if (entity.Has<BaseComponent>())
                    {
                        ref var baseComponent = ref entity.GetComponent<BaseComponent>();
                        
                        healthBar.DOKill();
                
                        healthBar
                            .DOValue(GetHealthOnPercent(health, maxHealth), ANIMATION_DURATION)
                            .SetAutoKill(true);

                        baseComponent.healthView.text = $"{health}/{maxHealth}";
                    }
                }
            }
        }

        private float GetHealthOnPercent(float currentHealth, float maxHealth)
        {
            return currentHealth / maxHealth;
        }
    }
}