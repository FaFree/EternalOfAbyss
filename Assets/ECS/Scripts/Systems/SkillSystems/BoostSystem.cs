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
        private Event<TextViewRequest> textRequest;

        public override void OnAwake()
        {
            boosts = WorldModels.Default.Get<Boosts>();

            playerFitler = this.World.Filter.With<PlayerComponent>();

            boostRequest = this.World.GetEvent<BoostRequest>();
            textRequest = this.World.GetEvent<TextViewRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!boostRequest.IsPublished)
                return;

            var playerEntity = playerFitler.FirstOrDefault();

            if (playerEntity == default)
                return;

            foreach (var evt in boostRequest.BatchedChanges)
            {
                var playerModel = WorldModels.Default.Get<UnitPlayer>();
                var boostModel = WorldModels.Default.Get<BoostsModel>();

                var boost = boosts.BoostsMap[evt.boostKey];

                if (boost.isReboundBoost || boost.isTripleArrow || boost.isPassingArrow)
                {
                    boostModel.isTripleArrow = boostModel.isTripleArrow || boost.isTripleArrow;
                    boostModel.isReboundArrow = boostModel.isReboundArrow || boost.isReboundBoost;
                    boostModel.isPassingArrow = boostModel.isPassingArrow || boost.isPassingArrow;
                }
                
                boostModel.boosts.Add(boost);

                playerModel.AddBoost(boosts.BoostsMap[evt.boostKey]);

                playerEntity.GetComponent<HealthComponent>().health += boosts.BoostsMap[evt.boostKey].heal;

                var playerPosition = playerEntity.GetComponent<TransformComponent>().transform.position;

                var spawnPosition = playerPosition + Vector3.up * 0.5f;
                
                textRequest.NextFrame(new TextViewRequest
                {
                    text = "Boost Added!",
                    position = playerEntity.GetComponent<TransformComponent>().transform.position
                });

                var go = Instantiate(boost.boostEffect, spawnPosition,
                    Quaternion.identity);

                var entity = this.World.CreateEntity();
                
                entity.SetComponent(new EffectMarker
                {
                    playTime = boost.effectPlayTime,
                    effectObject = go,
                    currentTime = 0f
                });
            }
        }
    }
}