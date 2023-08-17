using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class DestroyUnitSystem : UpdateSystem
    {
        private Event<DestroyUnitRequestEvent> destroyUnitRequestEvent;
        private Event<DestroyedUnitEvent> destroyedUnitEvent;
        public override void OnAwake()
        {
            this.destroyUnitRequestEvent = this.World.GetEvent<DestroyUnitRequestEvent>();
            this.destroyedUnitEvent = this.World.GetEvent<DestroyedUnitEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!destroyUnitRequestEvent.IsPublished)
                return;

            foreach (var evt in destroyUnitRequestEvent.BatchedChanges)
            {
                if (this.World.TryGetEntity(evt.entityId, out var entity))
                {
                    ref var entityTransform = ref entity.GetComponent<TransformComponent>().transform;

                    Destroy(entityTransform.gameObject);
                    this.World.RemoveEntity(entity);

                    this.destroyedUnitEvent.NextFrame(new DestroyedUnitEvent());
                }
                
                
            }
        }
    }
}