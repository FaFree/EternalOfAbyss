using System;
using Configs;
using DefaultNamespace;
using ECS.Scripts.Events;
using Models;
using Scellecs.Morpeh;
using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using Scripts.StorageService;
using UnityEngine;

namespace ECS.Scripts.Initializers
{
    [Serializable]
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        [SerializeField] private Prefabs prefabs;
        [SerializeField] private PlayerStatConfig playerConfig;
        [SerializeField] private Items items;
        [SerializeField] private BaseStatConfig baseStats;
        [SerializeField] private TurretStatConfig turretStats;
        [SerializeField] private BarrierStatConfig barrierStats;
        
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

                this.playerModel = new Player(playerConfig.config, 1f, 1.033f);
                
                WorldModels.Default.Set<Boosts>(boosts);
                WorldModels.Default.Set<Prefabs>(prefabs);
                WorldModels.Default.Set<Items>(items);
                WorldModels.Default.Set<Inventory>(inventory);
                WorldModels.Default.Set<Player>(playerModel);
                WorldModels.Default.Set<LevelsModel>(levelModel);
                WorldModels.Default.Set<BaseStatConfig>(baseStats);
                WorldModels.Default.Set<TurretStatConfig>(turretStats);
                WorldModels.Default.Set<BarrierStatConfig>(barrierStats);


                try
                {
                    var boostModel = storageService.Load<BoostsModel>("BoostModel");
                    WorldModels.Default.Set(boostModel);
                    boostModel.Initialize();
                }
                catch (Exception exception)
                {
                    BoostsModel boostModel = new BoostsModel();
                    WorldModels.Default.Set<BoostsModel>(boostModel);  
                }

                this.inventory.Initialize();
                
                this.endLoaded.NextFrame(new EndLoadEvent());
            }
        }
    }
}