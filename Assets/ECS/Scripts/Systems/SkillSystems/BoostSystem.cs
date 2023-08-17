using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

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
                ref var playerModel = ref playerEntity.GetComponent<PlayerComponent>().UnitPlayerModel;
                
                playerModel.AddBoost(boosts.BoostsMap[evt.boostKey]);

                playerEntity.GetComponent<HealthComponent>().health += boosts.BoostsMap[evt.boostKey].heal;
                
                textRequest.NextFrame(new TextViewRequest
                {
                    text = "Boost Added!",
                    position = playerEntity.GetComponent<TransformComponent>().transform.position
                });
            }
        }
    }
}