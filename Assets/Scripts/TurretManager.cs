using System;
using System.Collections.Generic;
using Scripts.BoostFeature;
using UnityEngine;

namespace Scripts
{
    public class TurretManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Turrets;
        private BoostsModel boostModel;

        private void Start()
        {
            this.boostModel = WorldModels.Default.Get<BoostsModel>();

            foreach (var boost in this.boostModel.boosts)
            {
                if (boost.category == Categories.Turrets)
                {
                    Turrets[boost.turretNumber - 1].SetActive(true);
                }
            }
        }
    }
}