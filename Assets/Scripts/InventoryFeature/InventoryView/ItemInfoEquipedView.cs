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
        
        public Action onUpgradeClick;
        public Action onUnequipClick;
        
        private string itemId;
        private string itemKey;

        public void Initialize(Item item)
        {
            this.itemId = item.itemId;
            this.itemKey = item.key;

            this.textInfo.text = item.textInfo;
            this.upgradeCost.text = item.upgradeCost.ToString();

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
            
            this.transform.gameObject.SetActive(false);
        }
    }
}