using ECS.Scripts.Events;
using ECS.Scripts.Events.BankEvents;
using Scripts.LevelFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts
{
    public class LevelSystem : UpdateSystem
    {
        private Event<OnResourceChanged> onResourceChanged;
        private Event<OnLevelChanged> onLevelChanged;
        
        public override void OnAwake()
        {
            this.onResourceChanged = this.World.GetEvent<OnResourceChanged>();
            this.onLevelChanged = this.World.GetEvent<OnLevelChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!LevelManager.isView)
            {
                if (LevelManager.TryAddLevel())
                {
                    onLevelChanged.NextFrame(new OnLevelChanged
                    {
                        CurrentLevel = LevelManager.CurrentLevel
                    });

                    LevelManager.isView = true;
                }
            }
        }
    }
}