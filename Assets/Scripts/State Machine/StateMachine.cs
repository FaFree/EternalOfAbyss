using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private BaseState currentState;
    private Dictionary<Type, BaseState> statesMap = new Dictionary<Type, BaseState>();

    public void AddState(BaseState state)
    {
        this.statesMap.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : BaseState
    {
        var type = typeof(T);
        
        if (currentState != null && currentState.GetType() == type)
            return;
        
        if (statesMap.TryGetValue(type, out var newState))
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }

    public void Update()
    {
        this.currentState?.OnUpdate();
    }
}
