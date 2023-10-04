using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class TextViewSystem : UpdateSystem
    {
        private const string DAMAGE_PREFAB = "Assets/Addressables/DamageText.prefab";

        private Event<DamagedEvent> damagedEvent;
        private Event<BoostSpawnedEvent> boostSpawnedEvent;
        
        private GameObject prefab;

        public override void OnAwake()
        {
            this.damagedEvent = this.World.GetEvent<DamagedEvent>();
            this.boostSpawnedEvent = this.World.GetEvent<BoostSpawnedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (this.damagedEvent.IsPublished)
            {
                foreach (var evt in this.damagedEvent.BatchedChanges)
                {
                    if (!this.World.TryGetEntity(evt.EntityId, out var entity))
                    {
                        continue;
                    }
                    
                    var text = "-" + evt.Damage.ToString();
                    
                    var position = entity.GetComponent<TransformComponent>().transform.position;
                    
                    this.SpawnText(text, position + new Vector3(0, 2, 0));
                }
            }

            if (this.boostSpawnedEvent.IsPublished)
            {
                foreach (var evt in this.boostSpawnedEvent.BatchedChanges)
                {
                    if (!this.World.TryGetEntity(evt.EntityId, out var entity))
                    {
                        continue;
                    }

                    var text = "Boost Added!";

                    var position = entity.GetComponent<TransformComponent>().transform.position;
                    
                    this.SpawnText(text, position + new Vector3(0, 2, 0));
                }   
            }
        }

        private void SpawnText(string text, Vector3 position)
        {
            if (prefab.IsUnityNull())
                prefab = Addressables.LoadAssetAsync<GameObject>(DAMAGE_PREFAB).WaitForCompletion();
            
            var go = Instantiate(prefab, position, Quaternion.identity);

            var textObj = go.GetComponent<TextConfig>().textObject;

            textObj.GetComponent<TextMeshPro>().text = text;

            textObj.transform.DOMove(textObj.transform.position + Vector3.up, 1);
            
            textObj.transform.DOScale(Vector3.one * 0.5f, 1).OnComplete(() =>
            {
                textObj.transform.DOKill();
                Destroy(go);
            });
        }
    }
}