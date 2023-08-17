using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Events
{
    public struct LevelEndEvent : IEventData
    {
        public int levelNum;
        public GameObject levelGo;
        public Vector3 startPosition;
    }
}