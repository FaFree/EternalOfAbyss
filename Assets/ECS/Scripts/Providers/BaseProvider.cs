using System;
using DefaultNamespace;
using ECS.Scripts.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scripts;
using Scripts.BoostFeature;

namespace ECS.Scripts.Providers
{
    public class BaseProvider : MonoProvider<BaseComponent>
    {
        private void Start()
        {
            ref var baseComponent = ref this.Entity.GetComponent<BaseComponent>();

            var stats = WorldModels.Default.Get<BaseStatConfig>();
            
            float maxHealth = stats.maxHealth;
            baseComponent.regeneration = stats.regeneration;

            this.Entity.SetComponent(new HealthComponent
            {
                health = maxHealth,
                MaxHealth = maxHealth
            });
            
            this.Entity.SetComponent(new HealthBarComponent
            {
                CanvasTransform = baseComponent.CanvasTransform,
                HealthBarSlider = baseComponent.healthBarSlider
            });
            
            this.Entity.SetComponent(new TransformComponent
            {
                transform = baseComponent.position
            });
        }
    }
}