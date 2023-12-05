using CoinScaleSystem;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
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
                    Time.timeScale = 0f;
                    
                    var boostModel = WorldModels.Default.Get<BoostsModel>();
                    boostModel.Clear();
                    
                    this.World.RemoveEntity(entity);
                    
                    this.playerDestroyedEvent.NextFrame(new PlayerDestroyedEvent());
                }
            }
        }
    }
}