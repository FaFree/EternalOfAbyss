using System;
using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using Scripts.StorageService;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Initializers
{
    [Serializable]
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        [FormerlySerializedAs("levels")] [SerializeField] private Prefabs prefabs;
        [SerializeField] private PlayerStatConfig playerConfig;
        [SerializeField] private Items items;
        [SerializeField] private BaseStatConfig baseStats;
        private Event<EndLoadEvent> endLoaded;

        private Inventory inventory = new Inventory();
        private Player playerModel;
        private LevelsModel levelModel = new LevelsModel();

        private IStorageService storageService;

        public override void OnAwake()
        {
            this.endLoaded = this.World.GetEvent<EndLoadEvent>();
            
            this.storageService = new JsonFileStorageService();
            
            if (WorldModels.Default.isNull())
            {
                this.boosts.Initialize();
                this.prefabs.Initialize();
                this.items.Initialize();

              //  try
              //  {
              //      var config = storageService.Load<UnitConfig>("PlayerModel");
              //      playerModel = new UnitPlayer(config, 0.7f, 1.267f);
              //  }
              //  catch
              //  {
              //      playerModel = new UnitPlayer(playerConfig.config, 0.7f, 1.267f);
              //  }
                
                this.playerModel = new Player(playerConfig.config, 1f, 1.033f);

                WorldModels.Default.Set<Boosts>(boosts);
                WorldModels.Default.Set<Prefabs>(prefabs);
                WorldModels.Default.Set<Items>(items);
                WorldModels.Default.Set<Inventory>(inventory);
                WorldModels.Default.Set<Player>(playerModel);
                WorldModels.Default.Set<LevelsModel>(levelModel);
                WorldModels.Default.Set<BaseStatConfig>(baseStats);
        
                BoostsModel boostModel = new BoostsModel();
                WorldModels.Default.Set<BoostsModel>(boostModel);
                
            
                this.inventory.Initialize();
                
                this.endLoaded.NextFrame(new EndLoadEvent());
            }
        }
    }
}