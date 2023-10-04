using DG.Tweening;
using ECS.Scripts.Components;
using ECS.Scripts.Events;
using ECS.Scripts.Events.BankEvents;
using Scripts.LevelFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts
{
    public class LevelViewSystem : UpdateSystem
    {
        private const string exp = "Exp";
        
        private Filter levelViewFilter;

        private Event<OnLevelChanged> onLevelChanged;
        
        private Event<OnResourceChanged> onResourceChanged;

        public override void OnAwake()
        {
            this.levelViewFilter = this.World.Filter.With<LevelViewComponent>();

            this.onLevelChanged = this.World.GetEvent<OnLevelChanged>();
            this.onResourceChanged = this.World.GetEvent<OnResourceChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var levelEntity in levelViewFilter)
            {
                ref var levelViewComponent = ref levelEntity.GetComponent<LevelViewComponent>();
                
                var image = levelViewComponent.image;
                var text = levelViewComponent.text;

                if (this.onResourceChanged.IsPublished)
                {
                    var res = Resources.GetResource(exp);
                
                    image.DOFillAmount((float)(res.ResourceCount / LevelManager.GetRequiredXp()), 2);
                }
                
                if (this.onLevelChanged.IsPublished)
                    text.text = $"LV. {LevelManager.CurrentLevel}";
            }
        }
    }
}