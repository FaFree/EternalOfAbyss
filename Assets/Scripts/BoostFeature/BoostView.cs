using System;
using DG.Tweening;
using ECS.Scripts.Events;
using ResourceFeature;
using Scripts;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Resources = ResourceFeature.Resources;

public class BoostView : MonoBehaviour
{
    [SerializeField] private Image image;
    
    [SerializeField] private Button button;
    
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillInfo;
    [SerializeField] private TextMeshProUGUI statInfo;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI counter;
    
    [SerializeField] private GameObject counterObj;

    [SerializeField] private RectTransform completeRoot;
    [SerializeField] private RectTransform unavailableRoot;
    
    private Resource coins;
    
    private Transform parent;

    private string boostKey;

    public Boost Boost;

    public Action<Boost, Object> onClick;

    public void Init(Boost boost)
    {
        this.Boost = boost;
        
        this.image.sprite = this.Boost.sprite;
        this.skillName.text = this.Boost.skillName;
        this.skillInfo.text = this.Boost.skillInfo;

        int price = (int)this.Boost.price;
        this.price.text = price.ToString();
        
        this.statInfo.text = this.Boost.ToString();

        if (this.Boost.isActive)
        {
            this.completeRoot.gameObject.SetActive(true);
            this.button.interactable = false;

            return;
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

        if (this.Boost.purchaseCount > 0)
        {
            this.counterObj.SetActive(true);
            this.counter.text = this.Boost.purchaseCount.ToString();
        }
    }

    private void Awake()
    {
        this.coins = Resources.GetResource("Coin");
        
        this.button.interactable = true;
    }

    public void OnClick()
    {
        if (DOTween.IsTweening(this.gameObject.transform) || DOTween.IsTweening(counter.gameObject.transform))
            return;
        
        this.onClick?.Invoke(this.Boost, this);
    }

    public void Multiply()
    {
        this.Boost.Multiply();

        this.counter.gameObject.transform.DOPunchScale(Vector3.up * 0.2f, 0.1f);
    }
}
