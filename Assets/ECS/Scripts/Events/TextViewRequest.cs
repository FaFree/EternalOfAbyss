using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct TextViewRequest : IEventData
    {
        public Vector3 position;
        public string text;
    }
}