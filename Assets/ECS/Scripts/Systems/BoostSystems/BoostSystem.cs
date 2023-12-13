using Configs;
using DefaultNamespace;
using ECS.Scripts.Events;
using Models;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts.BoostFeature;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;

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
                var playerModel = WorldModels.Default.Get<Player>();

                playerModel.AddBoost(evt.boost);

                switch (evt.boost.category)
                {
                  case  Categories.Base:
                      var baseStats = WorldModels.Default.Get<BaseStatConfig>();
                      baseStats.regeneration += evt.boost.baseBoostConfig.regeneration;
                      baseStats.maxHealth += evt.boost.baseBoostConfig.health;
                      break;
                  
                  case Categories.Barriers:
                      var barrierStats = WorldModels.Default.Get<BarrierStatConfig>();
                      barrierStats.health += evt.boost.barrierBoostConfig.health;
                      break;
                  
                  case Categories.Turrets:
                      var turretStats = WorldModels.Default.Get<TurretStatConfig>();
                      turretStats.damage += evt.boost.turretBoostConfig.damage;
                      break;
                  case Categories.Weapons:
                      var itemsMap = WorldModels.Default.Get<Items>().ItemsMap;
                      var inventory = WorldModels.Default.Get<Inventory>();
                      
                      inventory.AddItem(itemsMap[evt.boost.weaponConfig.weaponKey]);
                      
                      inventory.Equip(itemsMap[evt.boost.weaponConfig.weaponKey]);
                      break;
                }
            }
        }
    }
}