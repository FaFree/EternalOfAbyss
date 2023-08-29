using System;
using Scripts.InventoryFeature;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public Action OnClicked { get; set; }
        
        private Inventory inventory;

        private Item currentItem;
        
        public string ItemKey { get; set; }
        public string ItemId { get; set; }

        public void Initialize()
        {
            this.inventory = WorldModels.Default.Get<Inventory>();

            this.currentItem = this.inventory.GetItemOrDefault(ItemKey);
            
            this.image.sprite = this.currentItem.sprite;
        }

        public void OnClick()
        {
            this.OnClicked?.Invoke();
        }

        public void SetVisible(bool state)
        {
            this.image.enabled = state;
        }
    }
}