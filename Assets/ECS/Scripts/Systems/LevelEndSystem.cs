using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class LevelEndSystem : UpdateSystem
    {
        private Filter zoneFilter;
        private Filter levelEndFilter;
        private Filter playerFilter;

        private bool isLevelEmpty;

        private Event<LevelEndEvent> levelEndEvent;
        public override void OnAwake()
        {
            this.zoneFilter = this.World.Filter.With<ZoneComponent>();
            this.levelEndFilter = this.World.Filter.With<LevelEndComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();

            this.levelEndEvent = this.World.GetEvent<LevelEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            var playerEntity = playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;

            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

            foreach (var levelEntity in levelEndFilter)
            {
                ref var levelComponent = ref levelEntity.GetComponent<LevelEndComponent>();
                
                var endPosition = levelComponent.endPosition.position;

                var distance = Vector3.Magnitude(playerTransform.position - endPosition);

                if (distance < 1f)
                {
                    if (zoneFilter.FirstOrDefault() == default)
                        isLevelEmpty = true;
                    
                    foreach (var zoneEntity in zoneFilter)
                    {
                        ref var zoneComponent = ref zoneEntity.GetComponent<ZoneComponent>();

                        if (zoneComponent.currentUnitCount == 0)
                        {
                            isLevelEmpty = true;
                        }
                        else
                        {
                            isLevelEmpty = false;
                            break;
                        }
                    }

                    if (isLevelEmpty)
                    {
                        levelEndEvent.NextFrame(new LevelEndEvent
                        {
                            levelNum = levelComponent.levelNum,
                            levelGo = levelComponent.endPosition.gameObject,
                            startPosition = levelComponent.endPosition.position
                        });
                        return;
                    }
                }
                
            }
        }
    }
}