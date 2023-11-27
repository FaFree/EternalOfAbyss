using System.Collections.Generic;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Resources = ResourceFeature.Resources;

namespace Scripts.BoostFeature
{
    public class BoostWindowStart : MonoBehaviour
    {
        private const string ICON_KEY = "SkillIcon";

        [SerializeField] private RectTransform skillRoot;
        [SerializeField] private ButtonManager buttonManager;

        private List<BoostView> boostViews;
        
        private GameObject skillObj;
        
        private void Start()
        {
            this.buttonManager.onCategoryChanged += UpdateCategory;
            
            this.boostViews = new List<BoostView>();

            var key = WorldModels.Default.Get<Prefabs>().prefabMap[ICON_KEY];

            this.skillObj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            foreach (var boost in WorldModels.Default.Get<Boosts>().BoostsList)
            {
                var obj = Instantiate(skillObj, skillRoot);

                var boostView = obj.GetComponent<BoostView>();
                
                this.boostViews.Add(boostView);
                
                boostView.Init(boost);

                boostView.onClick += UpdateInfo;
                boostView.onAddedBoost += OnAddedBoost;
            }
        }

        private void OnAddedBoost(Boost boost)
        {
            var obj = Instantiate(skillObj, skillRoot);

            var boostView = obj.GetComponent<BoostView>();
            
            this.boostViews.Add(boostView);
            
            boostView.Init(boost);

            boostView.onAddedBoost += OnAddedBoost;
            boostView.onClick += UpdateInfo;
        }

        private void UpdateCategory(Categories category)
        {
            foreach (var boostView in this.boostViews)
            {
                boostView.gameObject.SetActive(false);
            }
            
            switch (category)
            {
                case Categories.All: 
                    foreach (var boostView in this.boostViews)
                    {
                        boostView.gameObject.SetActive(true);
                    }

                    break;
                
                case Categories.Available:
                    foreach (var boostView in this.boostViews)
                    {
                        if (Resources.GetResource("Coin").IsEnough(boostView.Boost.price))
                        {
                            boostView.gameObject.SetActive(!boostView.Boost.isActive);
                        }
                    }

                    break;
                
                default:
                    foreach (var boostView in this.boostViews)
                    {
                        boostView.gameObject.SetActive(boostView.Boost.category == category);
                    }

                    break;
            }
            this.UpdateInfo();
        }

        private void UpdateInfo()
        {
            foreach (var boostView in this.boostViews)
            {
                boostView.Init(boostView.Boost);
            }
        }
    }
}