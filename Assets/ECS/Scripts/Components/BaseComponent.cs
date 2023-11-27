using System;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct BaseComponent : IComponent
    {
        public Transform position;

        public float regeneration;

        public TextMeshProUGUI healthView;

        public Transform CanvasTransform;

        public Slider healthBarSlider;
    }
}