using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    public struct HealthBarComponent : IComponent
    {
        public Slider HealthBarSlider;
        public Transform CanvasTransform;
    }
}