using System;
using DG.Tweening;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct ResourceComponent : IComponent
    {
        public string resourceType;
        
        public Transform transform;
        
        public int reward;
    }
}