using UnityEngine;

public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState> {
    ChaseEnemy Enemy;

    public EnemyAttackState(EnemyStateMachine.EnemyState key, BasicEnemy Enemy) : base(key) {
        this.Enemy = (ChaseEnemy)Enemy;
    }

    public override void Enter() { }

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() {
        if (Enemy.IsInAttackRangeRange)
            return Key;
        else if (Enemy.IsAggroed)
            return EnemyStateMachine.EnemyState.Chase;
        else
            return EnemyStateMachine.EnemyState.Idle;
    }

    public override void Update() {
        Enemy.AttackTimer -= Time.deltaTime;

        if (Enemy.AttackTimer <= 0f) {
            Enemy.Attack();
            Enemy.AttackTimer = Enemy.AttackSpeed;
        }
    }
}