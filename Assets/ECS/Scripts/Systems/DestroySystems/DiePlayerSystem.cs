using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class DiePlayerSystem : UpdateSystem
    {
        private Event<BaseDieRequestEvent> playerDieRequestEvent;
        private Event<PlayerDestroyRequestEvent> playerDestroyRequestEvent;
        
        public override void OnAwake()
        {
            this.playerDestroyRequestEvent = this.World.GetEvent<PlayerDestroyRequestEvent>();
            this.playerDieRequestEvent = this.World.GetEvent<BaseDieRequestEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!playerDieRequestEvent.IsPublished)
                return;

            foreach (var evt in playerDieRequestEvent.BatchedChanges)
            {
                playerDestroyRequestEvent.NextFrame(new PlayerDestroyRequestEvent
                {
                    entityId = evt.entityId
                });
            }
        }
    }
}