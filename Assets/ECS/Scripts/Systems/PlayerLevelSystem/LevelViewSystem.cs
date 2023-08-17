using ECS.Scripts.Components;
using ECS.Scripts.Events;
using Factory.LevelFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts
{
    public class LevelViewSystem : UpdateSystem
    {
        private const string exp = "Exp";
        
        private Filter levelViewFilter;

        private Event<OnLevelChanged> onLevelChanged;

        public override void OnAwake()
        {
            this.levelViewFilter = this.World.Filter.With<LevelViewComponent>();

            this.onLevelChanged = this.World.GetEvent<OnLevelChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var levelEntity in levelViewFilter)
            {
                ref var levelViewComponent = ref levelEntity.GetComponent<LevelViewComponent>();
                var image = levelViewComponent.image;
                var text = levelViewComponent.text;
                
                var res = Resources.GetResource(exp);

                image.fillAmount = (float) (res.ResourceCount / LevelManager.GetRequiredXp());
                
                if (onLevelChanged.IsPublished)
                    text.text = $"LVL {LevelManager.CurrentLevel}";
            }
        }
    }
}