using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    [Serializable]
    public struct BarrierConfig
    {
        public float barrierHealth;
        
        public Transform barrierTransform;

        public GameObject barrierCanvas;

        public Slider barrierHealthBar;
    }
}