using ECS.Scripts.Components;
using ECS.Scripts.Events.BankEvents;
using ResourceFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts
{
    public class CoinUpdateSystem : UpdateSystem
    {
        private Filter coinUIFilter;
        
        private Event<OnResourceChanged> onResourceChanged;
        
        public override void OnAwake()
        {
            this.coinUIFilter = this.World.Filter.With<UiCoinComponent>();
            
            this.onResourceChanged = this.World.GetEvent<OnResourceChanged>();
            
            this.onResourceChanged.NextFrame(new OnResourceChanged());
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.onResourceChanged.IsPublished)
                return;

            foreach (var coinEntity in this.coinUIFilter)
            {
                ref var text = ref coinEntity.GetComponent<UiCoinComponent>().coinText;

                var resourceCoin = Resources.GetResource("Coin");

                text.text = resourceCoin.ToString();
            }
        }
    }
}