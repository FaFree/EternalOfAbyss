using System;
using ResourceFeature;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Resources = ResourceFeature.Resources;

namespace Scripts
{
    public class CollectDiamondView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        
        private LevelsModel levelsModel;

        private int level;
        
        private void Start()
        {
            levelsModel = WorldModels.Default.Get<LevelsModel>();
            level = levelsModel.GetCurrentLevel();
            textMeshPro.text += " " + (int) (10 * Math.Pow(2, 0.1 * level));
        }

        public void OnClick()
        {
            var res = Resources.GetResource("Diamond");
            res.AddResource(10 * Math.Pow(2, 0.1 * level));
            SceneManager.LoadScene("MenuScene");
        }
    }
}