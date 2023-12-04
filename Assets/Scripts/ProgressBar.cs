using System;
using Scripts;
using UnityEngine;

namespace DefaultNamespace
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private ProgressBarConfig config;

        private void Start()
        {
            WorldModels.Default.Set(config);
        }
    }
}