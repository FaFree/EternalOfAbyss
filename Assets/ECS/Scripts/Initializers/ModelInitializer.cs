using Scripts;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Initializers
{
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        [FormerlySerializedAs("levels")] [SerializeField] private Prefabs prefabs;
        [SerializeField] private Items items;
        
        private Inventory inventory = new Inventory();
        
        public override void OnAwake()
        {
            this.boosts.Initialize();
            this.prefabs.Initialize();
            this.items.Initialize();
            
            WorldModels.Default.Set<Boosts>(boosts);
            WorldModels.Default.Set<Prefabs>(prefabs);
            WorldModels.Default.Set<Items>(items);
            WorldModels.Default.Set<Inventory>(inventory);
            
            this.inventory.Initialize();
        }
    }
}