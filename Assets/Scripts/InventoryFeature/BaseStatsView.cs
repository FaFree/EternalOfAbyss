using DefaultNamespace;
using Scripts.LevelModel;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.InventoryFeature
{
    public class BaseStatsView : MonoBehaviour
    {
        private RectTransform rect;
        private TextMeshProUGUI textMesh;
        
        private GameObject infoPanel;

        private BaseStatConfig baseStats;

        private void Start()
        {
            var key = WorldModels.Default.Get<Prefabs>().prefabMap["PlayerInfo"];

            var go = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

            this.infoPanel = Instantiate(go);
            
            this.infoPanel.SetActive(false);
            
            this.infoPanel.transform.SetParent(this.gameObject.transform.parent);

            var infoConfig = infoPanel.GetComponent<PlayerInfoPanelConfig>();

            this.rect = infoConfig.rect;
            this.textMesh = infoConfig.text;

            this.baseStats = WorldModels.Default.Get<BaseStatConfig>();
        }


        public void OnPlayerClick()
        {
            this.infoPanel.SetActive(true);

            this.rect.anchoredPosition = new Vector2(0, 0);
            
            textMesh.text = this.baseStats.ToString();
        }
    }
}