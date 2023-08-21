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
            var go = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            
            go.transform.LookAt(direction);

            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ArrowComponent
            {
                damage = damage,
                speed = 5,
                direction = direction,
                isRebound = isRebound,
                collisionCount = 4
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = go.transform
            });

            if (isTripleArrow)
            {
                float angleFirstArrow = go.transform.rotation.eulerAngles.y;
                
                float angleOffSet = 45f;
                float angle1 = angleFirstArrow + angleOffSet;
                float angle2 = angleFirstArrow - angleOffSet;
                
                Quaternion rotation1 = Quaternion.Euler(0, angle1, 0);
                Quaternion rotation2 = Quaternion.Euler(0, angle2, 0);

                var go1 = Instantiate(arrowPrefab, spawnPosition, rotation1);
                var go2 = Instantiate(arrowPrefab, spawnPosition, rotation2);

                var entity1 = this.World.CreateEntity();
                var entity2 = this.World.CreateEntity();
                
                entity1.SetComponent(new ArrowComponent
                {
                    damage = damage,
                    collisionCount = 4,
                    speed = 5,
                    direction = go1.transform.forward,
                    isRebound = isRebound
                });
                
                entity2.SetComponent(new ArrowComponent
                {
                    damage = damage,
                    collisionCount = 4,
                    speed = 5,
                    direction = go2.transform.forward,
                    isRebound = isRebound
                });
                
                entity1.SetComponent(new TransformComponent
                {
                    transform = go1.transform
                });
                
                entity2.SetComponent(new TransformComponent
                {
                    transform = go2.transform
                });
            }
        }
    }
}