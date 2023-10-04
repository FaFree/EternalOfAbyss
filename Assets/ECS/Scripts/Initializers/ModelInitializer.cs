using DefaultNamespace;
using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using Scripts.PlayerUpgradeFeature;
using Scripts.StorageService;
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
        private LevelsModel levelModel = new LevelsModel();

        private IStorageService storageService;

        public override void OnAwake()
        {
            storageService = new JsonFileStorageService();
            
            if (WorldModels.Default.isNull())
            {
                this.boosts.Initialize();
                this.prefabs.Initialize();
                this.items.Initialize();
                this.upgradeModel.Initialize(upgrades.upgrades);

              //  try
              //  {
              //      var config = storageService.Load<UnitConfig>("PlayerModel");
              //      playerModel = new UnitPlayer(config, 0.7f, 1.267f);
              //  }
              //  catch
              //  {
              //      playerModel = new UnitPlayer(playerConfig.config, 0.7f, 1.267f);
              //  }
                
                playerModel = new UnitPlayer(playerConfig.config, 0.7f, 1.267f);

                WorldModels.Default.Set<Boosts>(boosts);
                WorldModels.Default.Set<Prefabs>(prefabs);
                WorldModels.Default.Set<Items>(items);
                WorldModels.Default.Set<Inventory>(inventory);
                WorldModels.Default.Set<BoostsModel>(boostModel);
                WorldModels.Default.Set<UpgradeModel>(upgradeModel);
                WorldModels.Default.Set<UnitPlayer>(playerModel);
                WorldModels.Default.Set<LevelsModel>(levelModel);
            
                this.inventory.Initialize();
            }
        }
    }
}