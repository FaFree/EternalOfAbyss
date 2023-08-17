using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class DestroyPlayerSystem : UpdateSystem
    {
        private Event<PlayerDestroyRequestEvent> playerDestroyRequestEvent;
        private Event<PlayerDestroyedEvent> playerDestroyedEvent;

        public override void OnAwake()
        {
            this.playerDestroyRequestEvent = this.World.GetEvent<PlayerDestroyRequestEvent>();
            this.playerDestroyedEvent = this.World.GetEvent<PlayerDestroyedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!playerDestroyRequestEvent.IsPublished)
                return;

            foreach (var evt in playerDestroyRequestEvent.BatchedChanges)
            {
                if (this.World.TryGetEntity(evt.entityId, out var entity))
                {
                    ref var playerTransform = ref entity.GetComponent<TransformComponent>().transform;
                    Destroy(playerTransform.gameObject);
                    this.World.RemoveEntity(entity);

                    playerDestroyedEvent.NextFrame(new PlayerDestroyedEvent());
                }
            }
        }
    }
}