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

        private Quaternion angle = Quaternion.Euler(40, 0, 0);

        private const float ANIMATION_DURATION = 0.2f;

        private Event<DamagedEvent> damagedEvent;

        public override void OnAwake()
        {
            this.entityFilter = this.World.Filter
                .With<HealthComponent>()
                .With<HealthBarComponent>();

            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
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
                }
            }
        }

        private float GetHealthOnPercent(float currentHealth, float maxHealth)
        {
            return currentHealth / maxHealth;
        }
    }
}