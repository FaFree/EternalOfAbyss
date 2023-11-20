using System.Collections.Generic;
using Scripts.BoostFeature;
using UnityEngine;

namespace Scripts
{
    public class BarrierManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Barriers;
        
        private BoostsModel boostModel;

        private void Start()
        {
            this.boostModel = WorldModels.Default.Get<BoostsModel>();

            int index = 0;

            foreach (var boost in this.boostModel.boosts)
            {
                if (boost.category == Categories.Barriers)
                {
                    Barriers[index++].SetActive(true);
                }
            }
        }
    }
}