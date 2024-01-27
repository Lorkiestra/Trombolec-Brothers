using UnityEngine;

public class EnemyDieState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy enemy;

    public EnemyDieState(EnemyStateMachine.EnemyState key, BasicEnemy enemy) : base(key) {
        this.enemy = enemy;
    }

    public override void Enter() => enemy.Die();

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() => Key;

    public override void Update() { }
}