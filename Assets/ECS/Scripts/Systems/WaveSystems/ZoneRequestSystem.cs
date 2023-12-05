using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class ZoneRequestSystem : UpdateSystem
    {
        private const string KEY = "WaveMenu";

        private Entity waveEntity;

        private GameObject waveMenu;
        private GameObject WaveMenuPrefab;
        
        private Event<WaveEndEvent> waveEndEvent;
        private Event<NextWaveRequest> nextWaveRequest;

        public override void OnAwake()
        {
            var adr = WorldModels.Default.Get<Prefabs>().prefabMap[KEY];

            this.WaveMenuPrefab = Addressables.LoadAssetAsync<GameObject>(adr).WaitForCompletion();
            
            this.waveEndEvent = this.World.GetEvent<WaveEndEvent>();
            this.nextWaveRequest = this.World.GetEvent<NextWaveRequest>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            if (!this.waveEndEvent.IsPublished)
                return;

            foreach (var evt in this.waveEndEvent.BatchedChanges)
            {
                if (!this.World.TryGetEntity(evt.waveEntityId, out var waveEntity))
                {
                    return;
                }

                this.waveMenu = Instantiate(WaveMenuPrefab);

                var waveView = this.waveMenu.GetComponent<WaveView>();

                this.waveEntity = waveEntity;
                
                waveView.onReadyClick += OnReadyClick;

                Time.timeScale = 0f;
            }
        }
        
        private void OnReadyClick()
        {
            Destroy(this.waveMenu);

            Time.timeScale = 1f;
            
            this.nextWaveRequest.NextFrame(new NextWaveRequest
            {
                zoneEntityId = this.waveEntity.ID,
            });
        }

    }
}