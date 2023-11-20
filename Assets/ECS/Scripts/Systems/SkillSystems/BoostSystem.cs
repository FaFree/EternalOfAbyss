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
        
        private Event<BoostRequest> boostRequest;

        public override void OnAwake()
        {
            this.boosts = WorldModels.Default.Get<Boosts>();
            
            this.boostRequest = this.World.GetEvent<BoostRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.boostRequest.IsPublished)
                return;

            foreach (var evt in this.boostRequest.BatchedChanges)
            {
                var boostModel = WorldModels.Default.Get<BoostsModel>();

                var boost = boosts.BoostsMap[evt.boostKey];
                
                boostModel.AddBoost(boost);
            }
        }
    }
}