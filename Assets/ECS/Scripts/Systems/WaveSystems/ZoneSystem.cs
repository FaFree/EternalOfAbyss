using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.VisualScripting;

namespace ECS.Scripts.Components
{
    public class ZoneSystem : UpdateSystem
    {
        private Event<UnitSpawnRequest> unitSpawnRequest;
        private Event<DieRequestEvent> dieRequestEvent;
        private Event<WaveEndEvent> waveEndEvent;
        
        private Filter zoneFilter;

        public override void OnAwake()
        {
            this.zoneFilter = this.World.Filter
                .With<ZoneComponent>();

            this.unitSpawnRequest = this.World.GetEvent<UnitSpawnRequest>();
            this.dieRequestEvent = this.World.GetEvent<DieRequestEvent>();
            this.waveEndEvent = this.World.GetEvent<WaveEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (this.dieRequestEvent.IsPublished)
            {
                foreach (var evt in this.dieRequestEvent.BatchedChanges)
                {
                    if (this.World.TryGetEntity(evt.entityId, out var entity))
                    {
                        if (entity.Has<UnitComponent>())
                        {
                            ref var zone = ref entity.GetComponent<UnitComponent>().zone.GetComponent<ZoneComponent>();
                            zone.currentUnitCount--;
                        }
                    }
                }
            }

            foreach (var zoneEntity in this.zoneFilter)
            {
                ref var zoneComponent = ref zoneEntity.GetComponent<ZoneComponent>();

                ref var slider = ref zoneComponent.waveProgressConfig.progressBar;
                
                var timePerMob = zoneComponent.allUnitSpawnTime / zoneComponent.maxUnitCount;

                zoneComponent.timer += deltaTime;

                if (zoneComponent.timer > timePerMob && !zoneComponent.isSpawned)
                {
                    this.unitSpawnRequest.NextFrame(new UnitSpawnRequest
                    {
                        zone = zoneEntity,
                        config = zoneComponent.config
                    });

                    zoneComponent.timer = 0f;
                    zoneComponent.currentUnitCount++;
                    zoneComponent.spawnedUnitCount++;

                    slider.DOKill();
                    
                    slider.DOValue(zoneComponent.spawnedUnitCount / (float)zoneComponent.maxUnitCount, 1f);
                    
                    zoneComponent.isSpawned = zoneComponent.spawnedUnitCount == zoneComponent.maxUnitCount;
                }

                if (zoneComponent.currentUnitCount == 0 && !zoneComponent.nextWave.IsUnityNull() 
                                                        && zoneComponent.maxUnitCount == zoneComponent.spawnedUnitCount
                                                        && !zoneComponent.isEnded)
                {
                    this.waveEndEvent.NextFrame(new WaveEndEvent
                    {
                        waveEntityId = zoneEntity.ID,
                    });

                    zoneComponent.isEnded = true;
                }
            }
        }
    }
}