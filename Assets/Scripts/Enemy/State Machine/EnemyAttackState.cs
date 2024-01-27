using UnityEngine;

public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState> {
    ChaseEnemy enemy;

    public EnemyAttackState(EnemyStateMachine.EnemyState key, BasicEnemy Enemy) : base(key) {
        this.enemy = (ChaseEnemy)Enemy;
    }

    public override void Enter() { }

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() {
        if (enemy.IsInAttackRangeRange)
            return Key;
        else if (enemy.IsAggroed)
            return EnemyStateMachine.EnemyState.Chase;
        else
            return EnemyStateMachine.EnemyState.ResetPatrol;
    }

    public override void Update() {
        enemy.AttackTimer -= Time.deltaTime;

        if (enemy.AttackTimer <= 0f) {
            enemy.Attack();
            enemy.AttackTimer = enemy.AttackSpeed;
        }
    }
}