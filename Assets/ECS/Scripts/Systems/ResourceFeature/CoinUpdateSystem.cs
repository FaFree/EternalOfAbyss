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
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!onResourceChanged.IsPublished)
                return;

            foreach (var coinEntity in uiFilter)
            {
                foreach (var evt in onResourceChanged.BatchedChanges)
                {
                    if (evt.ResourceName == "Coin")
                    {
                        ref var text = ref coinEntity.GetComponent<UiCoinComponent>().text;

                        var resource = Resources.GetResource(evt.ResourceName);

                        text.text = resource.ResourceCount.ToString();
                    }
                }
            }
        }
    }
}