using DefaultNamespace;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class BoostSystem : UpdateSystem
    {
        private Boosts boosts;
        
        private Filter playerFitler;

        private Event<BoostRequest> boostRequest;
        private Event<BoostSpawnedEvent> boostSpawnedEvent;


        public override void OnAwake()
        {
            this.boosts = WorldModels.Default.Get<Boosts>();

            this.playerFitler = this.World.Filter.With<PlayerComponent>();

            this.boostRequest = this.World.GetEvent<BoostRequest>();
            this.boostSpawnedEvent = this.World.GetEvent<BoostSpawnedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.boostRequest.IsPublished)
                return;

            var playerEntity = this.playerFitler.FirstOrDefault();

            if (playerEntity == default)
                return;

            foreach (var evt in this.boostRequest.BatchedChanges)
            {
                var playerModel = WorldModels.Default.Get<UnitPlayer>();
                
                var boostModel = WorldModels.Default.Get<BoostsModel>();

                var boost = boosts.BoostsMap[evt.boostKey];
                
                boostModel.AddBoost(boost);

                playerModel.AddBoost(boosts.BoostsMap[evt.boostKey]);

                playerEntity.GetComponent<HealthComponent>().health += boosts.BoostsMap[evt.boostKey].heal;

                var playerPosition = playerEntity.GetComponent<TransformComponent>().transform.position;

                var spawnPosition = playerPosition + Vector3.up * 0.5f;
                
                var go = Instantiate(boost.boostEffect, spawnPosition,
                    Quaternion.identity);

                var entity = this.World.CreateEntity();
                
                entity.SetComponent(new EffectMarker
                {
                    playTime = boost.effectPlayTime,
                    effectObject = go,
                    currentTime = 0f
                });
                
                this.boostSpawnedEvent.NextFrame(new BoostSpawnedEvent
                {
                    EntityId = playerEntity.ID,
                    Boost = boost
                });
            }
        }
    }
}