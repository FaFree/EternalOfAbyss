using System;
using Scripts.InventoryFeature;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryFeature.InventoryView
{
    public class CurrentItemView : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public Action OnClicked { get; set; }

        public void UpdateSprite(Sprite sprite)
        {
            this.image.sprite = sprite;
        }

        public void OnClick()
        {
            this.OnClicked?.Invoke();
        }
    }
}