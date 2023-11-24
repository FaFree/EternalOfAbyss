using DefaultNamespace;
using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class HealthBarSystem : UpdateSystem
    {
        private Filter entityFilter;

        private Quaternion angle = Quaternion.Euler(30, 0, 0);

        private const float ANIMATION_DURATION = 0.2f;
        private float timer;

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
            timer += deltaTime;
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
                    foreach (var evt in damagedEvent.BatchedChanges)
                    {
                        healthBar.DOKill();
                
                        healthBar
                            .DOValue(GetHealthOnPercent(health, maxHealth), ANIMATION_DURATION)
                            .SetAutoKill(true);

                        if (this.World.TryGetEntity(evt.EntityId, out var damagedEntity))
                        {
                            if (damagedEntity.Has<BaseComponent>() && entity.Has<BaseComponent>())
                            {
                                ref var baseComponent = ref damagedEntity.GetComponent<BaseComponent>();

                                baseComponent.healthView.text = $"{(int)health}/{(int)maxHealth}";
                        
                                if (timer > 0.1f)
                                {
                                    baseComponent.healthView.transform.DOPunchPosition(Vector3.up * 0.2f, 0.1f);
                                    this.timer = 0f;
                                }
                            }
                        }
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

                        baseComponent.healthView.text = $"{(int)health}/{(int)maxHealth}";
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