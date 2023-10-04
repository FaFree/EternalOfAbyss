using System;
using Scripts.InventoryFeature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryFeature.InventoryView
{
    public class ItemInfoEquipedView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textInfo;
        [SerializeField] private TextMeshProUGUI upgradeCost;
        [SerializeField] private Image itemIcon;
        [SerializeField] private RectTransform rect;

        [SerializeField] public Button upgradeButton;
        
        public Action onUpgradeClick;
        public Action onUnequipClick;
        
        private string itemId;
        private string itemKey;

        public void Initialize(Item item)
        {
            this.itemId = item.itemId;
            this.itemKey = item.key;

            this.textInfo.text = item.textInfo;

            int cost = (int) item.upgradeCost;
            
            this.upgradeCost.text = cost.ToString();

            this.itemIcon.sprite = item.sprite;

            var pos = new Vector2(0, 0);
            
            this.rect.anchoredPosition = pos;
            
            this.transform.gameObject.SetActive(true);
        }

        public void OnUpgradeClick()
        {
            onUpgradeClick?.Invoke();
        }

        public void OnUnequipClick()
        {
            onUnequipClick?.Invoke();
        }

        public void Reset()
        {
            onUpgradeClick = null;
            onUnequipClick = null;
            
            this.upgradeButton.interactable = true;
            this.transform.gameObject.SetActive(false);
        }
    }
}