using System.Linq;
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
        private Filter playerFilter;
        
        private Event<ArrowRequest> arrowRequest;
        
        private GameObject arrowPrefab;

        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter.With<PlayerComponent>();
            
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();

            var arrowAddress = WorldModels.Default.Get<Prefabs>().prefabMap["Arrow"];

            this.arrowPrefab = Addressables.LoadAssetAsync<GameObject>(arrowAddress).WaitForCompletion();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!arrowRequest.IsPublished)
                return;

            var playerEntity = playerFilter.FirstOrDefault();

            ref var boostComponent = ref playerEntity.GetComponent<BoostComponent>();
            
            foreach (var evt in arrowRequest.BatchedChanges)
            {
                SpawnArrow(evt.spawnPosition, evt.direction, evt.damage, boostComponent.isTripleArrow, boostComponent.isReboundArrow);
            }
        }

        private void SpawnArrow(Vector3 spawnPosition, Vector3 direction, float damage, bool isTripleArrow, bool isRebound)
        {
            var playerEntity = playerFilter.FirstOrDefault();
            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

            var go = Instantiate(arrowPrefab, spawnPosition,
                Quaternion.Euler(0, playerTransform.rotation.eulerAngles.y, 0));
            
            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ArrowComponent
            {
                damage = damage,
                speed = 10,
                direction = direction.normalized,
                isRebound = isRebound,
                collisionCount = 4,
                maxDuration = 10,
                currentDuration = 0
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = go.transform
            });

            if (isTripleArrow)
            {
                float angleFirstArrow = playerTransform.rotation.eulerAngles.y;

                float angleOffSet = 15f;
                float angle1 = angleFirstArrow + angleOffSet;
                float angle2 = angleFirstArrow - angleOffSet;

                Quaternion rotation1 = Quaternion.Euler(0, angle1, 0);
                Quaternion rotation2 = Quaternion.Euler(0, angle2, 0);

                var go1 = Instantiate(arrowPrefab, spawnPosition, rotation1);
                var go2 = Instantiate(arrowPrefab, spawnPosition, rotation2);

                var entity1 = this.World.CreateEntity();
                var entity2 = this.World.CreateEntity();
                
                SpawnEntity(damage, 3, 10, go1.transform.forward.normalized, isRebound, go1.transform);
                SpawnEntity(damage, 3, 10, go2.transform.forward.normalized, isRebound, go2.transform);

            }
        }

        private void SpawnEntity(float damage, int collisionCount, float speed, Vector3 direction, bool isRebound, Transform transform)
        {
            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ArrowComponent
            {
                collisionCount = collisionCount,
                damage = damage,
                speed = speed,
                direction = direction,
                isRebound = isRebound,
                maxDuration = 10,
                currentDuration = 0
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = transform
                
            });
        }
        
    }
}