using System;
using ECS.Scripts.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ECS.Scripts.Providers
{
    public class BarrierProvider : MonoProvider<BarrierComponent>
    {
        private void Start()
        {
            ref var barrierComponent = ref this.Entity.GetComponent<BarrierComponent>();
            
            this.Entity.SetComponent(new TransformComponent
            {
                transform = barrierComponent.barrierConfig.barrierTransform
            });
            
            this.Entity.SetComponent(new HealthComponent
            {
                health = barrierComponent.barrierConfig.barrierHealth,
                MaxHealth = barrierComponent.barrierConfig.barrierHealth
            });
            
            this.Entity.SetComponent(new HealthBarComponent
            {
                CanvasTransform = barrierComponent.barrierConfig.barrierCanvas.transform,
                HealthBarSlider = barrierComponent.barrierConfig.barrierHealthBar
            });
        }
    }
}