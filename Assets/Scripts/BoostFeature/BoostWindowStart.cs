using System.Collections.Generic;
using DG.Tweening;
using ECS.Scripts.Events;
using ResourceFeature;
using Scellecs.Morpeh;
using Scripts.LevelModel;
using Unity.VisualScripting;
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
        
        private Resource coins;

        private Event<BoostRequest> boostRequest;
        
        private GameObject skillObj;
        
        private void Start()
        {
            this.buttonManager.onCategoryChanged += UpdateCategory;

            this.boostRequest = World.Default.GetEvent<BoostRequest>();
            
            this.boostViews = new List<BoostView>();

            this.coins = Resources.GetResource("Coin");

            var key = WorldModels.Default.Get<Prefabs>().prefabMap[ICON_KEY];

            this.skillObj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            foreach (var boost in WorldModels.Default.Get<Boosts>().BoostsList)
            {
                var obj = Instantiate(skillObj, skillRoot);

                var boostView = obj.GetComponent<BoostView>();
                
                this.boostViews.Add(boostView);
                
                boostView.Init(boost);

                boostView.onClick += OnClick;
            }
        }

        public void OnClick(Boost boost, Object sender)
        {
            var boostModel = WorldModels.Default.Get<BoostsModel>();
            var boostView = (BoostView) sender;

            if (this.coins.IsEnough(boost.price))
            {
                if (boost.isMultiply && !boost.isActive)
                {
                    var boostCopy = boost.Copy();
                
                    boostCopy.Activate();
                    
                    boostModel.AddBoost(boostCopy);
                    
                    this.boostRequest.NextFrame(new BoostRequest
                    {
                        boost = boostCopy
                    });
                
                    boostView.Multiply();

                    this.coins.TakeResource(boostCopy.price);

                    this.UpdateInfo();

                    return;
                }

                else
                {
                    this.boostRequest.NextFrame(new BoostRequest
                    {
                        boost = boost,
                    });
                    
                    boost.Activate();
                    
                    boostModel.AddBoost(boost);
                    
                    this.coins.TakeResource(boost.price);
                    
                    this.UpdateInfo();

                    return;
                }
            }
            
            boostView.gameObject.transform.DOPunchPosition(Vector3.up * 10, 1);

            this.UpdateInfo();
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