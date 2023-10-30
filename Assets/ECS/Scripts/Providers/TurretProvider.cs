using ECS.Scripts.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ECS.Scripts.Providers
{
    public class TurretProvider : MonoProvider<TurretComponent>
    {
        private void Start()
        {
            this.Entity.SetComponent(new TransformComponent
            {
                transform = this.transform
            });
        }
    }
}