using System;
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

            var boostModel = WorldModels.Default.Get<BoostsModel>();
            
            foreach (var evt in arrowRequest.BatchedChanges)
            {
                var spawnPosition = new Vector3(evt.spawnPosition.x, 0, evt.spawnPosition.z);
                
                SpawnArrow(spawnPosition, evt.direction.normalized, evt.damage, 
                    boostModel.isTripleArrow, boostModel.isReboundArrow, boostModel.isPassingArrow);
            }
        }

        private void SpawnArrow(Vector3 spawnPosition, Vector3 direction, float damage, 
            bool isTripleArrow, bool isRebound, bool isPassing)
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
                currentDuration = 0,
                passingCount = 1,
                isPassing = isPassing
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = go.transform
            });

            if (isTripleArrow)
            {
                double angleFirstArrow = Math.Atan2(direction.z, direction.x);

                double angleOffSet = 30f;
                double angle1 = angleFirstArrow + angleOffSet;
                double angle2 = angleFirstArrow - angleOffSet;

                Quaternion rotation1 = Quaternion.Euler(0, (float)angle1, 0);
                Quaternion rotation2 = Quaternion.Euler(0, (float)angle2, 0);

                var go1 = Instantiate(arrowPrefab, spawnPosition, rotation1);
                var go2 = Instantiate(arrowPrefab, spawnPosition, rotation2);

                var entity1 = this.World.CreateEntity();
                var entity2 = this.World.CreateEntity();

                Vector3 direction1 = new Vector3((float) Math.Cos(angle1), 0, (float) Math.Sin(angle1));
                Vector3 direction2 = new Vector3((float) Math.Cos(angle2), 0, (float) Math.Sin(angle2));

                SpawnEntity(damage, 3, 10, direction1.normalized, isRebound, go1.transform, isPassing);
                SpawnEntity(damage, 3, 10, direction2.normalized, isRebound, go2.transform, isPassing);

            }
        }

        private void SpawnEntity(float damage, int collisionCount, float speed, 
            Vector3 direction, bool isRebound, Transform transform, bool isPassing)
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
                currentDuration = 0,
                isPassing = isPassing,
                passingCount = 1
            });
            
            entity.SetComponent(new TransformComponent
            {
                transform = transform
            });
        }
        
    }
}