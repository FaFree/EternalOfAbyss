using System;
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
            
            float maxHealth = baseComponent.maxHealth;

            foreach (var kvp in WorldModels.Default.Get<Boosts>().BoostsMap)
            {
                if (kvp.Value.category == Categories.Base)
                {
                    maxHealth += kvp.Value.health;
                    baseComponent.regeneration += kvp.Value.regeneration;
                }
            }

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