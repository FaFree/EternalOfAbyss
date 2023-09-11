using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class ZoneSystem : UpdateSystem
    {
        private Event<UnitSpawnRequest> unitSpawnRequest;

        private Filter zoneFilter;

        public override void OnAwake()
        {
            this.zoneFilter = this.World.Filter
                .With<ZoneComponent>();

            this.unitSpawnRequest = this.World.GetEvent<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var zoneEntity in zoneFilter)
            {
                ref var zoneComponent = ref zoneEntity.GetComponent<ZoneComponent>();

                if (!zoneComponent.isSpawned)
                {
                    for (int i = 0; i < zoneComponent.maxUnitCount; i++)
                    {
                        unitSpawnRequest.NextFrame(new UnitSpawnRequest
                        {
                            zone = zoneEntity,
                            config = zoneComponent.config
                        });
                        
                        zoneComponent.currentUnitCount++;
                        zoneComponent.isSpawned = true;
                    }
                    
                    zoneComponent.isSpawned = true;
                }
            }
        }
    }
}