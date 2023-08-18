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
    public class ArrowSpawnSystem : UpdateSystem
    {
        private Event<ArrowRequest> arrowRequest;
        private GameObject arrowPrefab;

        public override void OnAwake()
        {
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();

            var arrowAddress = WorldModels.Default.Get<Prefabs>().prefabMap["Arrow"];

            this.arrowPrefab = Addressables.LoadAssetAsync<GameObject>(arrowAddress).WaitForCompletion();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!arrowRequest.IsPublished)
                return;

            foreach (var evt in arrowRequest.BatchedChanges)
            {
                SpawnArrow(evt.spawnPosition, evt.direction, evt.damage);
            }
        }

        private void SpawnArrow(Vector3 spawnPosition, Vector3 direction, float damage)
        {
            var go = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            
            go.transform.LookAt(direction);

            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ArrowComponent
            {
                damage = damage,
                duration = 5,
                direction = direction
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = go.transform
            });

            go.transform.DOMove(direction, 5, false);
        }
    }
}