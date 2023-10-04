using System;
using System.Collections.Generic;
using ResourceFeature;
using Scripts;
using Scripts.InventoryFeature;
using Scripts.LevelModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Resources = ResourceFeature.Resources;

namespace InventoryFeature.InventoryView
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Image emptyImage;
        
        [SerializeField] private RectTransform inventoryRoot;
        [SerializeField] private RectTransform canvasRoot;

        [SerializeField] private CurrentItemView helmetImage;
        [SerializeField] private CurrentItemView chestImage;
        [SerializeField] private CurrentItemView weaponImage;
        [SerializeField] private CurrentItemView pantsImage;
        [SerializeField] private CurrentItemView bootsImage;
        [SerializeField] private CurrentItemView ringImage;
        
        private List<ItemView> itemViews;

        private Inventory inventory;
        
        private GameObject itemPrefab;

        private ItemInfoView itemInfoView;

        private ItemInfoEquipedView itemInfoEquipedView;
        
        private Dictionary<ItemType, CurrentItemView> equippedInventoryViews;

        private void Start()
        {
            this.itemViews = new List<ItemView>();
            
            this.inventory = WorldModels.Default.Get<Inventory>();

            var key = WorldModels.Default.Get<Prefabs>().prefabMap["IconItem"];

            var infoKey = WorldModels.Default.Get<Prefabs>().prefabMap["ItemInfo"];

            var itemInfoEquipKey = WorldModels.Default.Get<Prefabs>().prefabMap["ItemEquipInfo"];

            var itemInfoPrefab = Addressables.LoadAssetAsync<GameObject>(infoKey).WaitForCompletion();
            var itemInfoEquipPrefab = Addressables.LoadAssetAsync<GameObject>(itemInfoEquipKey).WaitForCompletion();

            var itemInfo = Instantiate(itemInfoPrefab);
            var itemInfoEquip = Instantiate(itemInfoEquipPrefab);
            
            itemInfoEquip.transform.SetParent(canvasRoot);
            itemInfo.transform.SetParent(canvasRoot);

            this.itemInfoView = itemInfo.GetComponent<ItemInfoView>();
            this.itemInfoEquipedView = itemInfoEquip.GetComponent<ItemInfoEquipedView>();
            
            this.itemInfoView.Reset();
            this.itemInfoEquipedView.Reset();

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
                kvp.Value.OnClicked += () =>
                {
                    itemInfoEquipedView.Reset();
                    itemInfoEquipedView.Initialize(this.inventory.CurrentItems[kvp.Key]);
                    itemInfoView.Reset();
                    itemInfoEquipedView.onUpgradeClick += () => Upgrade(this.inventory.CurrentItems[kvp.Key]);
                    itemInfoEquipedView.onUnequipClick += () => UnEquipInventory(kvp.Key);
                };
            }
            
            UpdateInventory();
        }

        private void Upgrade(Item item)
        {
            inventory.TryUpgradeItem(item.itemId);

            var res = Resources.GetResource("Coin");

            if (!res.IsEnough(item.upgradeCost))
            {
                itemInfoEquipedView.upgradeButton.interactable = false;
            }

            if (item.currentItemLevel == item.itemStats.maxLevel)
            {
                itemInfoEquipedView.upgradeButton.interactable = false;
            }
            
            itemInfoEquipedView.Initialize(item);
        }
        private void UnEquipInventory(ItemType itemType)
        {
            this.inventory.UnEquip(itemType);
            
            itemInfoEquipedView.Reset();
            
            this.UpdateInventory();
        }

        private void EquipItem(Item item)
        {
            this.inventory.Equip(item);
            
            this.UpdateInventory();
        }

        private void OpenInfoPanel(ItemView itemView)
        {
            itemInfoEquipedView.Reset();
            
            var item = this.inventory.GetItemOrDefault(itemView.ItemId);
            
            itemInfoView.Initialize(item.sprite, item.key, item.textInfo);

            itemInfoView.onButtonClick += () =>
            {
                this.EquipItem(item);
                itemInfoView.Reset();
            };
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
                        () =>
                        {
                            itemInfoView.Reset();
                            this.OpenInfoPanel(newItemView);
                        };
                    
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
                if (item.Value == default)
                {
                    this.equippedInventoryViews[item.Key].SetActive(false);
                }
                else
                {
                    this.equippedInventoryViews[item.Key].SetActive(true);
                    this.equippedInventoryViews[item.Key].UpdateSprite(item.Value.sprite);
                }
            }
        }
    }
}