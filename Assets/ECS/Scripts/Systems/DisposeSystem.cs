using DG.Tweening;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class DisposeSystem : UpdateSystem
    {
        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void Dispose()
        {
            var filter = this.World.Filter.Without<AllMarker>();

            foreach (var entity in filter)
            {
                this.World.RemoveEntity(entity);
            }

            DOTween.KillAll();
            
            base.Dispose();
        }
    }
}