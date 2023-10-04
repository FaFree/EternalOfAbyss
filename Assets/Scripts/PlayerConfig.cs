using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts
{
    public class PlayerConfig : MonoBehaviour
    {
        public Transform CanvasHealthTransform;
        public Slider HealthBarSlider;
        public Transform WeaponRightRoot;
        public Transform WeaponLeftRoot;
    }
}