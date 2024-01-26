using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum {
    protected BaseState<EState> CurrentState;

    protected Dictionary<EState, BaseState<EState>> States = new();

    void Start() => CurrentState.Enter();

    void Update() {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.Key)) 
            CurrentState.Update();
        else 
            TransitionTo(nextStateKey);
    }

    public void TransitionTo(EState stateKey) {
        Debug.Log($"Transitioning from {CurrentState.Key} to {stateKey}");
        CurrentState.Exit();
        CurrentState = States[stateKey];
        CurrentState.Enter();
    }
}