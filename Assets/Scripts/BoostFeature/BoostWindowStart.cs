using System;
using System.Collections.Generic;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.BoostFeature
{
    public class BoostWindowStart : MonoBehaviour
    {
        [SerializeField] private RectTransform skillRoot;
        [SerializeField] private ButtonManager buttonManager;

        private List<BoostView> boostViews;
        
        private GameObject skillObj;
        
        private void Start()
        {
            this.buttonManager.onCategoryChanged += UpdateCategory;
            
            this.boostViews = new List<BoostView>();

            var key = WorldModels.Default.Get<Prefabs>().prefabMap["SkillIcon"];

            this.skillObj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            foreach (var kvp in WorldModels.Default.Get<Boosts>().BoostsMap)
            {
                var obj = Instantiate(skillObj, skillRoot);

                var boostView = obj.GetComponent<BoostView>();
                
                this.boostViews.Add(boostView);
                
                boostView.Init(kvp.Key);

                boostView.onClick += UpdateInfo;
            }
        }

        private void UpdateCategory(Categories category)
        {
            if (category == Categories.All)
            {
                foreach (var boostView in boostViews)
                {
                    boostView.gameObject.SetActive(true);
                }

                return;
            }
            
            foreach (var boostView in this.boostViews)
            {
                boostView.gameObject.SetActive(boostView.Boost.category == category);
            }
            
            this.UpdateInfo();
        }

        private void UpdateInfo()
        {
            foreach (var boostView in this.boostViews)
            {
                boostView.Init(boostView.Boost.key);
            }
        }
    }
}