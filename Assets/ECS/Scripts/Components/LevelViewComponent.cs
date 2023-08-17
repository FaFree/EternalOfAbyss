using System;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct LevelViewComponent : IComponent
    {
        public Image image;
        public TextMeshProUGUI text;
    }
}