using System;
using Configs;
using Models;
using Scripts;
using Scripts.BoostFeature;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuilderFeature
{
    public class BuildItemView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI counterRoot;

        public string ghostObjectKey;
        public string objectKey;
        public string buildingTag;

        public Categories category;
        
        public Action<BuildItemView> onClick;

        public int itemsCount { get; private set; }

        public void Add(Boost boost)
        {
            if (this.itemsCount == 0)
                this.Init(boost);
            
            this.itemsCount++;

            this.counterRoot.text = this.itemsCount.ToString();
        }

        public void Init(Boost boost)
        {
            var turretConfig = WorldModels.Default.Get<TurretStatConfig>();
            var barrierConfig = WorldModels.Default.Get<BarrierStatConfig>();
            
            switch (boost.category)
            {
                case Categories.Turrets: 
                    this.buildingTag = boost.turretBoostConfig.buildingTag;
                    this.ghostObjectKey = turretConfig.ghostPrefab;
                    this.objectKey = turretConfig.prefab;
                    break;
                
                case Categories.Barriers:
                    this.buildingTag = boost.barrierBoostConfig.buildingTag;
                    this.ghostObjectKey = barrierConfig.ghostPrefab;
                    this.objectKey = barrierConfig.prefab;
                    break;
            }
            
            this.image.sprite = boost.sprite;
            this.category = boost.category;
        }
        
        
        public void AddItemCount()
        {
            this.itemsCount++;

            this.counterRoot.text = this.itemsCount.ToString();
        }

        public void TakeItemCount()
        {
            this.itemsCount--;

            this.counterRoot.text = this.itemsCount.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.onClick?.Invoke(this);
            
            this.counterRoot.text = this.itemsCount.ToString();
        }
    }
}