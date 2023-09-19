using System;
using System.Collections.Generic;
using ECS.Scripts.Events;
using InventoryFeature.InventoryView;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Scripts.InventoryFeature;
using Scripts.InventoryFeature.InventoryModel;
using Scripts.LevelModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ECS.Scripts.Components.ItemDropFeature
{
    public class ItemCollectViewSystem : UpdateSystem
    {
        private const string KEY = "ItemCollectInfo";
        
        private Event<ItemDroppedEvent> itemDropped;

        private ItemInfoView itemPanel;

        private Inventory inventory;

        public override void OnAwake()
        {
            this.itemDropped = this.World.GetEvent<ItemDroppedEvent>();

            this.inventory = WorldModels.Default.Get<Inventory>();

            var canvasObject = new GameObject("Canvas");
            
            canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var canvasRoot = canvasObject.GetComponent<Transform>();

            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            var pathKey = WorldModels.Default.Get<Prefabs>().prefabMap[KEY];
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(pathKey).WaitForCompletion();

            var go = Instantiate(prefab, canvasRoot);

            this.itemPanel = go.GetComponent<ItemInfoView>();
            
            this.itemPanel.Reset();
        }

        public override void OnUpdate(float deltaTime)
        {
            itemPanel.UpdateTime();
            
            if (!itemDropped.IsPublished)
                return;

            foreach (var evt in itemDropped.BatchedChanges)
            {
                Time.timeScale = 0f;
                
                var item = inventory.GetItemOrDefault(evt.ItemId);
                
                this.itemPanel.Initialize(item.sprite, item.key, item.textInfo);

                this.itemPanel.onButtonClick += () =>
                {
                    Time.timeScale = 1f;
                    this.itemPanel.Reset();
                };
            }
        }
    }
}