using Configs;
using ECS.Scripts.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scripts;

namespace ECS.Scripts.Providers
{
    public class TurretProvider : MonoProvider<TurretComponent>
    {
        private void Start()
        {
            ref var turretComponent = ref this.Entity.GetComponent<TurretComponent>();

            turretComponent.config.damage = WorldModels.Default.Get<TurretStatConfig>().damage;
            
            this.Entity.SetComponent(new TransformComponent
            {
                transform = this.transform
            });
        }
    }
}