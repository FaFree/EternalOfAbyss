using DefaultNamespace;
using ECS.Scripts.Events.InventoryEvents;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;

namespace ECS.Scripts.Components.InventorySystems
{
    public class EquipSystem : UpdateSystem
    {
        private Filter playerFilter;

        private Event<OnItemChanged> onItemChanged;
        
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter.With<PlayerComponent>();

            this.onItemChanged = this.World.GetEvent<OnItemChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!onItemChanged.IsPublished)
                return;

            var playerEntity = playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;
            
            WorldModels.Default.Get<Player>().ChangeItems();
        }
    }
}