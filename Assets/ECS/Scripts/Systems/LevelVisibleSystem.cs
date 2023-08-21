using System.Collections.Generic;
using System.Linq;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.LevelModel;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class LevelVisibleSystem : UpdateSystem
    {
        private Filter unitFilter;
        private Filter zoneFilter;
        private Filter levelFilter;
        
        private Event<LevelEndEvent> levelEndEvent;
        private Event<NavMeshUpdateRequest> updateRequest;

        private Dictionary<string, string> levels;
        public override void OnAwake()
        {
            this.unitFilter = this.World.Filter.With<UnitComponent>();
            this.levelFilter = this.World.Filter.With<LevelEndComponent>();
            
            this.levelEndEvent = this.World.GetEvent<LevelEndEvent>();
            this.updateRequest = this.World.GetEvent<NavMeshUpdateRequest>();

            this.levels = WorldModels.Default.Get<Prefabs>().prefabMap;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!levelEndEvent.IsPublished)
                return;
            
            foreach (var evt in levelEndEvent.BatchedChanges)
            {
                var newLevel = Addressables.LoadAssetAsync<GameObject>
                    (levels[$"Level{evt.levelNum + 1}"]).WaitForCompletion();

                foreach (var entity in unitFilter)
                {
                    this.World.RemoveEntity(entity);
                }

                foreach (var entity in levelFilter)
                {
                    this.World.RemoveEntity(entity);
                }

                var levelEndTransformPosition = evt.levelGo.transform.position;

                var position = new Vector3(levelEndTransformPosition.x - 10, levelEndTransformPosition.y + 0.5f,
                    levelEndTransformPosition.z);
                
                Destroy(evt.levelGo.transform.parent.gameObject);
                Instantiate(newLevel, position, Quaternion.identity);
                
                updateRequest.NextFrame(new NavMeshUpdateRequest{});
            }
        }
    }
}