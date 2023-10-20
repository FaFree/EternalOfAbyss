using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class LevelRestartSystem : UpdateSystem
    {
        private Event<LevelRestartRequest> restartRequest;

        private Filter zoneFilter;
        private Filter playerFilter;
        private Filter unitFilter;
        private Filter levelFilter;
        private Filter coinFilter;
        
        public override void OnAwake()
        {
            this.restartRequest = this.World.GetEvent<LevelRestartRequest>();

            this.zoneFilter = this.World.Filter.With<ZoneComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();
            this.levelFilter = this.World.Filter.With<LevelEndComponent>();
            this.coinFilter = this.World.Filter.With<ResourceComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!restartRequest.IsPublished)
                return;

            var zoneEntity = this.zoneFilter.FirstOrDefault();
            
            var playerEntity = this.playerFilter.FirstOrDefault();

            if (zoneEntity == default)
                return;

            ref var levelGo = ref zoneEntity.GetComponent<ZoneComponent>().Level;

            Vector3 position = levelGo.transform.position;

            Destroy(levelGo);
            
            foreach (var entity in unitFilter)
            {
                Destroy(entity.GetComponent<TransformComponent>().transform.gameObject);
                this.World.RemoveEntity(entity);
            }

            foreach (var entity in coinFilter)
            {
                entity.GetComponent<ResourceComponent>().transform.DOKill();
                entity.GetComponent<ResourceComponent>().transform.gameObject.SetActive(false);
                this.World.RemoveEntity(entity);
            }
            
            foreach (var entity in levelFilter)
            {
                this.World.RemoveEntity(entity);
            }
            
            var levels = WorldModels.Default.Get<Prefabs>().prefabMap;
            
            var newLevel = Addressables.LoadAssetAsync<GameObject>
                (levels[$"Level{WorldModels.Default.Get<LevelsModel>().GetCurrentLevel()}"]).WaitForCompletion();

            var go = Instantiate(newLevel, position, Quaternion.identity);

            playerEntity.GetComponent<TransformComponent>().transform.position = go.transform.position;

            playerEntity.GetComponent<HealthComponent>().health = WorldModels.Default.Get<UnitPlayer>().MaxHealth;
        }
    }
}