using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected readonly StateMachine stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void OnUpdate() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}
