using System;
using DG.Tweening;
using ECS.Scripts.Components;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ECS.Scripts
{
    public class ResourceSpawnSystem : UpdateSystem
    {
        private const int MAX_COUNT = 1;
        
        private const float DIE_ANIMATION_TIME = 3.25f;

        private const string COIN_PREFAB = "Assets/Addressables/coin.prefab";

        private Filter unitDieFilter;
        
        private Event<DieRequestEvent> dieRequest;
        private float radius = 1f;
        
        public override void OnAwake()
        {
            this.dieRequest = this.World.GetEvent<DieRequestEvent>();

            this.unitDieFilter = this.World.Filter.With<DieAnimationMarker>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (dieRequest.IsPublished)
            {
                foreach (var evt in dieRequest.BatchedChanges)
                {
                    if (this.World.TryGetEntity(evt.entityId, out var unitEntity))
                    {
                        unitEntity.AddComponent<DieAnimationMarker>();
                    }
                }
            }

            foreach (var unit in unitDieFilter)
            {
                if (unit.GetComponent<UnitComponent>().dieTime >= DIE_ANIMATION_TIME)
                {
                    ref var unitComponent = ref unit.GetComponent<UnitComponent>();
                    
                    var unitTransform = unit.GetComponent<TransformComponent>().transform;

                    var spawnPosition = unitTransform.position + unitTransform.forward * 2;

                    var coinReward = unitComponent.coinReward;
                    
                    var coinCount = Math.Min(coinReward, MAX_COUNT);
                    
                    var coinPrice = coinReward / coinCount;
                    
                    for (int i = 0; i < coinCount; i++)
                    {
                        SpawnResource(spawnPosition, coinPrice, COIN_PREFAB, "Coin", 0.15f);
                    }

                    unit.RemoveComponent<DieAnimationMarker>();
                }
                
                else
                {
                    unit.GetComponent<UnitComponent>().dieTime += deltaTime;
                }
            }
        }

        private void SpawnResource(Vector3 unitPosition, int reward, string prefab, string resourceType, float scaleMax)
        {
            var prefabGo = Addressables.LoadAssetAsync<GameObject>(prefab).WaitForCompletion();
            
            var go = Instantiate(prefabGo, unitPosition, Quaternion.identity);
            
            go.transform.DOJump(Random.insideUnitSphere * radius + unitPosition + Vector3.up, 3, 1, 0.5f, false);

            go.transform.DOScale(Vector3.one * scaleMax, 1).SetLoops(-1, LoopType.Yoyo);

            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ResourceComponent
            {
                transform = go.transform,
                reward = reward,
                resourceType = resourceType,
            });
        }
    }
}