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
        private const int MAX_COUNT = 10;
        
        private const float DIE_ANIMATION_TIME = 3.25f;

        private const string COIN_PREFAB = "Assets/Addressables/coin.prefab";
        private const string XP_PREFAB = "Assets/Addressables/YellowGem.prefab";

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

                    var xpReward = unitComponent.xpReward;
                    
                    var coinCount = Math.Min(coinReward, MAX_COUNT);
                    var xpCount = Math.Min(xpReward, MAX_COUNT);
                    
                    var coinPrice = coinReward / coinCount;
                    var xpPrice = xpReward / xpCount;
                    
                    for (int i = 0; i < coinCount; i++)
                    {
                        SpawnResource(spawnPosition, coinPrice, COIN_PREFAB, "Coin", 0.15f);
                    }

                    for (int i = 0; i < xpCount; i++)
                    {
                        SpawnResource(spawnPosition, xpPrice, XP_PREFAB, "Exp", 0.75f);
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

            var seq = DOTween.Sequence();
            
            go.transform.DOJump(Random.insideUnitSphere * radius + unitPosition + Vector3.up, 3, 1, 0.5f, false);

            var firstScale = go.transform.localScale;
            
            seq.Append(go.transform.DOScale(Vector3.one * scaleMax, 1));
            seq.Append(go.transform.DOScale(firstScale, 1));

            seq.SetLoops(-1);
            
            var entity = this.World.CreateEntity();
            
            entity.SetComponent(new ResourceComponent
            {
                transform = go.transform,
                reward = reward,
                resourceType = resourceType,
                sequenceId = seq.intId
            });
        }
    }
}