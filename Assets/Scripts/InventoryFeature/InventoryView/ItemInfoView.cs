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

        private bool active = false;

        public string ItemKey { get; set; }
        
        public Action onButtonClick;
        
        public void Initialize(Sprite sprite, string itemKey, string textInfo)
        {
            ItemKey = itemKey;
            iconItem.sprite = sprite;
            text.text = textInfo;
            rect.anchoredPosition = new Vector2(0, 0);
            
            this.gameObject.SetActive(true);

            this.active = true;
        }

        public void Reset()
        {
            this.gameObject.SetActive(false);
            onButtonClick = null;
            this.active = false;

            Time.timeScale = 1f;
        }

        public void UpdateTime()
        {
            if (this.active)
                Time.timeScale = 0f;
        }

        public void OnButtonClick()
        {
            onButtonClick?.Invoke();
        }

        public void OnCloseClick()
        {
            Reset();
        }
    }
}