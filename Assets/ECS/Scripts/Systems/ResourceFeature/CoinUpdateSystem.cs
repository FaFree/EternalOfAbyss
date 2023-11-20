using DG.Tweening;
using ECS.Scripts.Components;
using ECS.Scripts.Events.BankEvents;
using ResourceFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using TMPro;

namespace ECS.Scripts
{
    public class CoinUpdateSystem : UpdateSystem
    {
        private Filter uiFilter;
        
        private Event<OnResourceChanged> onResourceChanged;
        
        public override void OnAwake()
        {
            this.uiFilter = this.World.Filter.With<UiCoinComponent>();
            
            this.onResourceChanged = this.World.GetEvent<OnResourceChanged>();
            
            this.onResourceChanged.NextFrame(new OnResourceChanged());
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!onResourceChanged.IsPublished)
                return;

            foreach (var coinEntity in uiFilter)
            {
                ref var text = ref coinEntity.GetComponent<UiCoinComponent>().coinText;

                var resourceCoin = Resources.GetResource("Coin");

                text.text = resourceCoin.ToString();
            }
        }
    }
}