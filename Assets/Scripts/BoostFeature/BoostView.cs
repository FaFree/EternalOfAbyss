using System;
using DG.Tweening;
using ECS.Scripts.Events;
using ResourceFeature;
using Scripts;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Resources = ResourceFeature.Resources;

public class BoostView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillInfo;
    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private RectTransform completeRoot;
    [SerializeField] private RectTransform unavailableRoot;

    private Resource coins;
 
    private Event<BoostRequest> boostRequest;

    private Transform parent;

    private string boostKey;

    public Boost Boost => WorldModels.Default.Get<Boosts>().BoostsMap[this.boostKey];

    public Action onClick;

    public void Init(string boostKey)
    {
        this.boostKey = boostKey;
        
        this.image.sprite = this.Boost.sprite;
        this.skillName.text = this.Boost.skillName;
        this.skillInfo.text = this.Boost.skillInfo;
        this.price.text = this.Boost.price.ToString();

        if (this.Boost.isActive)
        {
            this.completeRoot.gameObject.SetActive(true);
            this.button.interactable = false;
        }

        if (!this.coins.IsEnough(this.Boost.price) && !this.Boost.isActive)
        {
            this.price.color = new Color(255/255, 150f/255f, 150f/255f, 1f);
            this.unavailableRoot.gameObject.SetActive(true);
        }
        else
        {
            this.price.color = new Color(1, 1, 1, 1);
            this.unavailableRoot.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        this.boostRequest = World.Default.GetEvent<BoostRequest>();

        this.coins = Resources.GetResource("Coin");
        
        this.button.interactable = true;
    }

    public void OnClick()
    {
        if (this.coins.IsEnough(this.Boost.price))
        {
            this.boostRequest.NextFrame(new BoostRequest
            {
                boost = this.Boost,
            });
    
            this.button.interactable = false;

            this.Boost.Activate();
        
            this.completeRoot.gameObject.SetActive(true);

            this.coins.TakeResource(this.Boost.price);
            
            onClick?.Invoke();

            return;
        }

        if (DOTween.IsTweening(this.gameObject.transform))
            return;

        this.gameObject.transform.DOPunchPosition(Vector3.up * 10, 1);
        
        onClick?.Invoke();
    }
}
