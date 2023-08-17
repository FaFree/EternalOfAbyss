using System;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;

namespace ECS.Scripts.Components
{
    
    [Serializable]
    public struct UiCoinComponent : IComponent
    {
        public TextMeshProUGUI text;
    }
}