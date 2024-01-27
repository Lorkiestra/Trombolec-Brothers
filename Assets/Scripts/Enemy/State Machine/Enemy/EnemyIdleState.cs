using UnityEngine;

public class EnemyIdleState : BaseState<EnemyStateMachine.EnemyState> {
    float timeout = 0f;

    public EnemyIdleState(EnemyStateMachine.EnemyState key) : base(key) { }

    public override void Enter() => timeout = Random.Range(0f, 5f);

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() => (timeout <= 0) ? EnemyStateMachine.EnemyState.Patrol : Key;
    
    public override void Update() => timeout -= Time.deltaTime;
}