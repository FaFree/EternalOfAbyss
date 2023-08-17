using System;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct LevelEndComponent : IComponent
    {
        public int levelNum;
        public Transform endPosition;
    }
}