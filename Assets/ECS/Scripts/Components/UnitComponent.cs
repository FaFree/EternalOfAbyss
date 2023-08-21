using DefaultNamespace;
using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public struct UnitComponent : IComponent
    {
        public Entity zone;

        public int coinReward;

        public int xpReward;

        public float dieTime;

        public Vector3 DirectionPosition;
        
        public Vector3 spawnPosition;
        
        public BaseUnit unit;
        
        public StateMachine stateMachine;
        
        public Animator animator;
    }
}