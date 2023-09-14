using DefaultNamespace;
using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using Scripts.PlayerUpgradeFeature;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Initializers
{
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        [FormerlySerializedAs("levels")] [SerializeField] private Prefabs prefabs;
        [SerializeField] private PlayerStatConfig playerConfig;
        [SerializeField] private Items items;
        [SerializeField] private UpgradesObject upgrades;
        
        private Inventory inventory = new Inventory();
        private BoostsModel boostModel = new BoostsModel();
        private UnitPlayer playerModel;
        private UpgradeModel upgradeModel = new UpgradeModel();
        private LevelModel levelModel = new LevelModel();
        
        public override void OnAwake()
        {
            if (WorldModels.Default.isNull())
            {
                this.boosts.Initialize();
                this.prefabs.Initialize();
                this.items.Initialize();
                this.upgradeModel.Initialize(upgrades.upgrades);
                
                playerModel = new UnitPlayer(playerConfig.config, 0.7f, 1.267f);

                WorldModels.Default.Set<Boosts>(boosts);
                WorldModels.Default.Set<Prefabs>(prefabs);
                WorldModels.Default.Set<Items>(items);
                WorldModels.Default.Set<Inventory>(inventory);
                WorldModels.Default.Set<BoostsModel>(boostModel);
                WorldModels.Default.Set<UpgradeModel>(upgradeModel);
                WorldModels.Default.Set<UnitPlayer>(playerModel);
                WorldModels.Default.Set<LevelModel>(levelModel);
            
                this.inventory.Initialize();
            }
        }
    }
}