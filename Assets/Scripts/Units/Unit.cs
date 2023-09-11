using System;
using ECS.Scripts.Components;
using Scripts;
using Scellecs.Morpeh;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class Unit : BaseUnit
    {
        public Unit(UnitConfig config, float firstAttackTime, float attackAnimationTime) 
            : base(config, firstAttackTime, attackAnimationTime)
        {
            
        }
    }
}