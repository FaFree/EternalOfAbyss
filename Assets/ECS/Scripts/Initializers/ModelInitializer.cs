using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using UnityEngine;

namespace ECS.Scripts.Initializers
{
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        [SerializeField] private Levels levels;
        [SerializeField] private Items items;
        
        private Inventory inventory = new Inventory();
        
        public override void OnAwake()
        {
            this.boosts.Initialize();
            this.levels.Initialize();
            this.items.Initialize();
            
            WorldModels.Default.Set<Boosts>(boosts);
            WorldModels.Default.Set<Levels>(levels);
            WorldModels.Default.Set<Items>(items);
            WorldModels.Default.Set<Inventory>(inventory);
            
            this.inventory.Initialize();
        }
    }
}