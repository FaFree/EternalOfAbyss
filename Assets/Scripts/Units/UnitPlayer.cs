using System;
using ECS.Scripts.Components;
using Factory;
using Scellecs.Morpeh;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class UnitPlayer : BaseUnit
    {
        public UnitPlayer(UnitConfig config, float firstAttackTime, float attackAnimationTime,  ref Entity entity) 
            : base(config, firstAttackTime, attackAnimationTime, ref entity)
        {
            
        }
    }
}