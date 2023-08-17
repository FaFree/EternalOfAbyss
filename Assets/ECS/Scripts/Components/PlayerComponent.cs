using System;
using DefaultNamespace;
using Scellecs.Morpeh;
using UnityEngine;

[Serializable]
public struct PlayerComponent : IComponent
{
    public float speed;
    public StateMachine stateMachine;
    public UnitPlayer UnitPlayerModel;
    public Vector3 direction;
    public Animator animator;
}
