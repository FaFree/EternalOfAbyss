using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class ZoneSpawnSystem : UpdateSystem
    {
        private Event<NextWaveRequest> waveRequest;

        public override void OnAwake()
        {
            this.waveRequest = this.World.GetEvent<NextWaveRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.waveRequest.IsPublished)
                return;

            foreach (var evt in this.waveRequest.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.zoneEntityId, out var zoneEntity))
                {
                    return;
                }
                
                ref var zoneComponent = ref zoneEntity.GetComponent<ZoneComponent>();
                ref var textNum = ref zoneComponent.waveProgressConfig.textNum;
                ref var slider = ref zoneComponent.waveProgressConfig.progressBar;
                
                zoneComponent.nextWave.SetActive(true);

                int.TryParse(textNum.text, out var number);

                textNum.text = (number + 1).ToString();
                
                slider.value = 0f;
                    
                this.World.RemoveEntity(zoneEntity);
            }
        }
    }
}