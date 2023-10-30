using DefaultNamespace;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct UnitComponent : IComponent
    {
        public Entity zone;

        public int coinReward;
        
        public float dieTime;
        
        public BaseUnit unit;
        
        public StateMachine stateMachine;
        
        public Animator animator;
    }
}