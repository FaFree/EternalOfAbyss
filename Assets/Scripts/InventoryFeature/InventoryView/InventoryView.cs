using System;
using System.Collections.Generic;
using Scripts;
using Scripts.InventoryFeature;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace InventoryFeature.InventoryView
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Sprite emptySprite;
        
        [SerializeField] private RectTransform inventoryRoot;

        [SerializeField] private CurrentItemView helmetImage;
        [SerializeField] private CurrentItemView chestImage;
        [SerializeField] private CurrentItemView weaponImage;
        [SerializeField] private CurrentItemView pantsImage;
        [SerializeField] private CurrentItemView bootsImage;
        [SerializeField] private CurrentItemView ringImage;
        
        private List<ItemView> itemViews;

        private Inventory inventory;
        
        private GameObject itemPrefab;
        
        private Dictionary<ItemType, CurrentItemView> equippedInventoryViews;

        private void Start()
        {
            this.itemViews = new List<ItemView>();
            
            this.inventory = WorldModels.Default.Get<Inventory>();

            var key = WorldModels.Default.Get<Prefabs>().prefabMap["IconItem"];

            this.itemPrefab = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            this.equippedInventoryViews = new Dictionary<ItemType, CurrentItemView>()
            {
                { ItemType.Helmet, this.helmetImage },
                { ItemType.Chest, this.chestImage },
                { ItemType.Weapon, this.weaponImage },
                { ItemType.Pants, this.pantsImage },
                { ItemType.Boots, this.bootsImage },
                { ItemType.Ring, this.ringImage },
            };

            foreach (var kvp in this.equippedInventoryViews)
            {
                kvp.Value.OnClicked += () => this.UnEquipInventory(kvp.Key);
            }
            
            UpdateInventory();
        }

        private void UnEquipInventory(ItemType itemType)
        {
            this.inventory.UnEquip(itemType);
            
            this.UpdateInventory();
        }

        private void EquipItem(Item item)
        {
            this.inventory.Equip(item);
            
            this.UpdateInventory();
        }

        private void UpdateInventory()
        {
            foreach (var itemView in this.itemViews)
            {
                itemView.SetVisible(false);
            }

            for (var i = 0; i < this.inventory.InventoryItems.Count; i++)
            {
                if (i >= this.itemViews.Count)
                {
                    var go = Instantiate(this.itemPrefab, this.inventoryRoot);

                    var newItemView = go.GetComponent<ItemView>();

                    newItemView.OnClicked += 
                        () => this.EquipItem(this.inventory.GetItemOrDefault(newItemView.ItemKey));
                    
                    this.itemViews.Add(newItemView);
                }

                var item = this.inventory.InventoryItems[i];
                var itemView = this.itemViews[i];
                
                itemView.ItemKey = item.key;
                itemView.ItemId = item.itemId;
                
                itemView.Initialize();
                
                itemView.SetVisible(!item.isEquip);
            }

            foreach (var item in inventory.CurrentItems)
            {
                this.equippedInventoryViews[item.Key]
                    .UpdateSprite(item.Value == default ? this.emptySprite : item.Value.sprite);
            }
        }
    }
}