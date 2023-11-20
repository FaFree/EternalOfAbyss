using System.Collections.Generic;
using DefaultNamespace;
using Scripts.LevelModel;
using Scripts.PlayerUpgradeFeature;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Resources = ResourceFeature.Resources;

namespace Scripts.UpgradeView
{
    public class UpgradeWindowView : MonoBehaviour
    {
        [SerializeField] private Transform root;

        private List<UpgradeIconView> upgrades;

        private GameObject upgradeIconPrefab;

        private UpgradeModel upgradeModel;

        private Player playerModel;
        private void Start()
        {
            this.playerModel = WorldModels.Default.Get<Player>();
            
            upgrades = new List<UpgradeIconView>();
            
            var keyIconPrefab = WorldModels.Default.Get<Prefabs>().prefabMap["UpgradeIcon"];

            this.upgradeIconPrefab = Addressables.LoadAssetAsync<GameObject>(keyIconPrefab).WaitForCompletion();

            this.upgradeModel = WorldModels.Default.Get<UpgradeModel>();
            
            foreach (var upgrade in this.upgradeModel.GetAllUpgrades())
            {
                var go = Instantiate(upgradeIconPrefab, root);

                var upgradeIconView = go.GetComponent<UpgradeIconView>();

                upgradeIconView.onClick += () =>
                {
                    this.Click(upgradeIconView);
                };
                
                upgradeIconView.Initialize(upgrade.Value);
                
                upgrades.Add(upgradeIconView);
            }
        }

        private void Click(UpgradeIconView iconView)
        {
            var res = Resources.GetResource("Diamond");

            if (res.IsEnough(iconView.currentUpgrade.upgradePrice))
            {
                res.TakeResource(iconView.currentUpgrade.upgradePrice);
                
                Debug.Log(res.ResourceCount);
                
                this.playerModel.UpgradeModel(iconView.currentUpgrade.upgradeType);
                
                iconView.Initialize(this.upgradeModel.GetCurrentUpgrade(iconView.currentUpgrade.upgradeType));
            }
        }
    }
}