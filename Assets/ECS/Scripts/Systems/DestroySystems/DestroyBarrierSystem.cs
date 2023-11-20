using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class DestroyBarrierSystem : UpdateSystem
    {
        private Event<DestroyBarrierRequest> destroyBarrierRequest;

        public override void OnAwake()
        {
            this.destroyBarrierRequest = this.World.GetEvent<DestroyBarrierRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.destroyBarrierRequest.IsPublished)
                return;

            foreach (var evt in this.destroyBarrierRequest.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.entityId, out var entity))
                {
                    continue;
                }

                ref var barrierComponent = ref entity.GetComponent<BarrierComponent>();

                barrierComponent.barrierConfig.barrierTransform.gameObject.SetActive(false);
                
                this.World.RemoveEntity(entity);
            }
        }
    }
}