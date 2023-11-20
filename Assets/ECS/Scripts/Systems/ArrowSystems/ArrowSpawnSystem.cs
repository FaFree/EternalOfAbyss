using System;
using System.Linq;
using DG.Tweening;
using ECS.Scripts.Events;
using ECS.Scripts.Providers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.LevelModel;
using Scripts.PullObjectFeature;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class ArrowSpawnSystem : UpdateSystem
    {
        private Event<ArrowRequest> arrowRequest;
        
        private GameObject arrowPrefab;

        private PullObject arrowPull;

        public override void OnAwake()
        {
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();

            var arrowAddress = WorldModels.Default.Get<Prefabs>().prefabMap["Arrow"];

            this.arrowPrefab = Addressables.LoadAssetAsync<GameObject>(arrowAddress).WaitForCompletion();
            
            GameObject arrowRoot = new GameObject("ArrowRoot");

            this.arrowPull = new PullObject(arrowPrefab, arrowRoot.transform, 5);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!arrowRequest.IsPublished)
                return;

            var boostModel = WorldModels.Default.Get<BoostsModel>();
            
            foreach (var evt in arrowRequest.BatchedChanges)
            {
                var spawnPosition = new Vector3(evt.spawnPosition.x, 0, evt.spawnPosition.z);
                
                SpawnArrow(spawnPosition, evt.direction.normalized, evt.damage, 
                    boostModel.isTripleArrow, boostModel.isReboundArrow, 
                    boostModel.isPassingArrow, evt.isPlayer, evt.isAutoArrow, evt.entityId);
            }
        }

        private void SpawnArrow(Vector3 spawnPosition, Vector3 direction, float damage, 
            bool isTripleArrow, bool isRebound, bool isPassing, bool isPlayer, bool isAutoArrow, EntityId entityId)
        {
            var go = this.arrowPull.GetFreeElement();

            var trail = go.GetComponent<ArrowConfig>().trailObject;
            
            trail.SetActive(false);
            
            go.transform.position = spawnPosition;
            
            trail.SetActive(true);

            go.transform.rotation = Quaternion.LookRotation(direction);

            var entity = go.GetComponent<ArrowProvider>().Entity;
            
            this.SetComponents(entity, damage, 4, 10, direction.normalized, isRebound, go.transform, isPassing, isPlayer);

            if (isAutoArrow)
            {
                entity.SetComponent(new AutoArrowMarker
                {
                    entityId = entityId
                });
            }

            if (isTripleArrow && isPlayer)
            {
                double angleFirstArrow = Math.Atan2(direction.z, direction.x);

                double angleOffSet = 15.0; 
                double angle1 = angleFirstArrow + Math.PI * angleOffSet / 180.0;
                double angle2 = angleFirstArrow - Math.PI * angleOffSet / 180.0;
                
                Vector3 direction1 = new Vector3((float) Math.Cos(angle1), 0, (float) Math.Sin(angle1));
                Vector3 direction2 = new Vector3((float) Math.Cos(angle2), 0, (float) Math.Sin(angle2));

                Quaternion rotation1 = Quaternion.LookRotation(direction1);
                Quaternion rotation2 = Quaternion.LookRotation(direction2);

                var go1 = this.arrowPull.GetFreeElement();

                var trail1 = go1.GetComponent<ArrowConfig>().trailObject;
                
                trail1.SetActive(false);
                go1.transform.position = spawnPosition;
                trail1.SetActive(true);
                go1.transform.rotation = rotation1;

                var go2 = this.arrowPull.GetFreeElement();

                var trail2 = go2.GetComponent<ArrowConfig>().trailObject;
                trail2.SetActive(false);
                go2.transform.position = spawnPosition;
                trail2.SetActive(true);

                go2.transform.rotation = rotation2;

                var entity1 = go1.GetComponent<ArrowProvider>().Entity;
                var entity2 = go2.GetComponent<ArrowProvider>().Entity;

                SetComponents(entity1, damage, 3, 10, direction1.normalized, isRebound, go1.transform, isPassing, isPlayer);
                SetComponents(entity2, damage, 3, 10, direction2.normalized, isRebound, go2.transform, isPassing, isPlayer);

            }
        }

        private void SetComponents(Entity entity, float damage, int collisionCount, float speed, 
            Vector3 direction, bool isRebound, Transform transform, bool isPassing, bool isPlayer)
        {
            if (isPlayer)
            {
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
                    passingCount = 1,
                });
            }
            else
            {
                entity.SetComponent(new ArrowComponent
                {
                    collisionCount = collisionCount,
                    damage = damage,
                    speed = speed,
                    direction = direction,
                    isRebound = false,
                    maxDuration = 10,
                    currentDuration = 0,
                    isPassing = false,
                    passingCount = 1
                });
            }
            
            
            entity.SetComponent(new TransformComponent
            {
                transform = transform
            });
        }
        
    }
}