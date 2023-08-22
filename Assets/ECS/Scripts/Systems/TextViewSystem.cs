using System.Collections.Generic;
using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.PlayerLoop;
using Sequence = DG.Tweening.Sequence;

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
                    SpawnText(evt.text, evt.position + new Vector3(2, 2, 0));
                }
            }
        }

        private void SpawnText(string text, Vector3 position)
        {
            if (prefab.IsUnityNull())
                prefab = Addressables.LoadAssetAsync<GameObject>(DAMAGE_PREFAB).WaitForCompletion();
            
            var go = Instantiate(prefab, position, Quaternion.identity);

            go.GetComponentInChildren<TextMeshPro>().text = text;

            var seq = DOTween.Sequence();
            
            seq.Append(go.transform.DOMove(position + Vector3.up, 1, false));
            seq.Join(go.transform.DOScale(Vector3.one * 0.5f, 1));
            
            seq.OnKill(() =>
            {
                Destroy(go);
            });
        }
    }
}