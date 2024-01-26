using System;

[Serializable]
public abstract class BaseState<EState> where EState : Enum {
    public EState Key { get; }

    public BaseState(EState key) => Key = key;

    public abstract void Enter();
    
    public abstract void Exit();

    public abstract void Update();

    public abstract EState GetNextState();
}