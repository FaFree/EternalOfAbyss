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
        private Event<TextViewRequest> textRequest;
        
        private const string DAMAGE_PREFAB = "Assets/Addressables/DamageText.prefab";

        private GameObject prefab;

        public override void OnAwake()
        {
            this.textRequest = this.World.GetEvent<TextViewRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (textRequest.IsPublished)
            {
                foreach (var evt in textRequest.BatchedChanges)
                {
                    SpawnText(evt.text, evt.position + new Vector3(0, 2, 0));
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