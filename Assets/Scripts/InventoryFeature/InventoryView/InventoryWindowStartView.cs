using System;
using System.Collections.Generic;
using DefaultNamespace;
using Scripts.BoostFeature;
using Scripts.LevelModel;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.InventoryFeature.InventoryView
{
    public class InventoryWindowStartView : MonoBehaviour
    {
        private const string ICON_KEY = "SkillIcon";
        
        [SerializeField] private RectTransform skillRoot;
        [SerializeField] private TextMeshProUGUI statRoot;
        [SerializeField] private ButtonManager buttonManager;
        
        private List<BoostView> boostViews;
        
        private GameObject skillObj;

        private void OnEnable()
        {
            this.buttonManager.onCategoryChanged += UpdateCategory;
            
            this.boostViews = new List<BoostView>();

            this.statRoot.text = WorldModels.Default.Get<Player>().ToString();
            
            buttonManager.Restart();
            
            var key = WorldModels.Default.Get<Prefabs>().prefabMap[ICON_KEY];

            this.skillObj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
            
            foreach (var boost in WorldModels.Default.Get<BoostsModel>().boosts)
            {
                var obj = Instantiate(skillObj, skillRoot);

                var boostView = obj.GetComponent<BoostView>();
                
                boostView.gameObject.SetActive(boostView.Boost.category == Categories.Player);
                
                this.boostViews.Add(boostView);
                
                boostView.Init(boost);
            }
        }

        private void Start()
        {
            this.buttonManager.onCategoryChanged += UpdateCategory;
            
            this.boostViews = new List<BoostView>();

            this.statRoot.text = WorldModels.Default.Get<Player>().ToString();

            var key = WorldModels.Default.Get<Prefabs>().prefabMap[ICON_KEY];

            this.skillObj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
            
            foreach (var boost in WorldModels.Default.Get<BoostsModel>().boosts)
            {
                var obj = Instantiate(skillObj, skillRoot);

                var boostView = obj.GetComponent<BoostView>();
                
                boostView.gameObject.SetActive(boostView.Boost.category == Categories.Player);

                this.boostViews.Add(boostView);
                
                boostView.Init(boost);
            }
        }

        private void UpdateCategory(Categories category)
        {
            foreach (var boostView in this.boostViews)
            {
                boostView.gameObject.SetActive(false);
            }
            
            switch (category)
            {
                case Categories.Base: 
                    foreach (var boostView in this.boostViews)
                    {
                        boostView.gameObject.SetActive(boostView.Boost.category != Categories.Player);
                    }

                    this.statRoot.text = WorldModels.Default.Get<BaseStatConfig>().ToString();

                    break;
                
                case Categories.Player:
                    foreach (var boostView in this.boostViews)
                    {
                        boostView.gameObject.SetActive(boostView.Boost.category == Categories.Player);
                    }

                    this.statRoot.text = WorldModels.Default.Get<Player>().ToString();

                    break;
            }
        }
    }
}