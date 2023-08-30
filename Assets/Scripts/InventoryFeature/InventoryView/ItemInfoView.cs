using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryFeature.InventoryView
{
    public class ItemInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image iconItem;
        [SerializeField] private RectTransform rect;

        public string ItemKey { get; set; }
        
        public Action onEquipClick;
        
        public void Initialize(Sprite sprite, string itemKey, string textInfo)
        {
            ItemKey = itemKey;
            iconItem.sprite = sprite;
            text.text = textInfo;
            rect.anchoredPosition = new Vector2(0, 0);
            
            this.gameObject.SetActive(true);
        }

        public void Reset()
        {
            this.gameObject.SetActive(false);
            onEquipClick = null;
        }

        public void OnEquipClick()
        {
            onEquipClick?.Invoke();
        }

        public void OnCloseClick()
        {
            Reset();
        }
    }
}