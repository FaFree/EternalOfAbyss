using DefaultNamespace;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts.BoostFeature;

namespace ECS.Scripts.Components
{
    public class BoostSystem : UpdateSystem
    {
        private Event<BoostRequest> boostRequest;

        public override void OnAwake()
        {
            this.boostRequest = this.World.GetEvent<BoostRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.boostRequest.IsPublished)
                return;

            foreach (var evt in this.boostRequest.BatchedChanges)
            {
                var boostModel = WorldModels.Default.Get<BoostsModel>();
                var playerModel = WorldModels.Default.Get<Player>();

                playerModel.AddBoost(evt.boost);
                boostModel.AddBoost(evt.boost);

                if (evt.boost.category == Categories.Base)
                {
                    var baseStats = WorldModels.Default.Get<BaseStatConfig>();

                    baseStats.regeneration += evt.boost.regeneration;
                    baseStats.maxHealth += evt.boost.health;
                }
            }
        }
    }
}