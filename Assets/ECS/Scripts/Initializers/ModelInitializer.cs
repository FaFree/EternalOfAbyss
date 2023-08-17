using Factory;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Initializers
{
    public class ModelInitializer : Initializer
    {
        [SerializeField] private Boosts boosts;
        
        public override void OnAwake()
        {
            this.boosts.Initialize();
            WorldModels.Default.Set<Boosts>(boosts);
        }
    }
}