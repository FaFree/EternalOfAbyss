using System;
using DefaultNamespace;
using Scripts.LevelModel;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.InventoryFeature
{
    public class PlayerStatsView : MonoBehaviour
    {
        private RectTransform rect;
        private TextMeshProUGUI textMesh;

        private UnitPlayer playerModel;

        private GameObject infoPanel;

        private void Start()
        {
            var key = WorldModels.Default.Get<Prefabs>().prefabMap["PlayerInfo"];

            var go = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            this.infoPanel = Instantiate(go);
            
            this.infoPanel.SetActive(false);
            
            this.infoPanel.transform.SetParent(this.gameObject.transform);

            var infoConfig = infoPanel.GetComponent<PlayerInfoPanelConfig>();

            this.rect = infoConfig.rect;
            this.textMesh = infoConfig.text;

            var model = WorldModels.Default.Get<UnitPlayer>();
            
            this.playerModel = model;
        }


        public void OnPlayerClick()
        {
            this.infoPanel.SetActive(true);

            this.rect.anchoredPosition = new Vector2(0, 0);
            
            this.playerModel.ChangeItem();

            textMesh.text = this.playerModel.ToString();
        }
    }
}