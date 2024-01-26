using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy Enemy;

    public EnemyChaseState(EnemyStateMachine.EnemyState key, BasicEnemy Enemy) : base(key) {
        this.Enemy = Enemy;
    }

    public override void Enter() => Enemy.SetAggro();

    public override void Exit() => Enemy.ClearAggro();

    public override EnemyStateMachine.EnemyState GetNextState() {
        if (Enemy.IsInAttackRangeRange) 
            return EnemyStateMachine.EnemyState.Attack;
        else if (Enemy.AggroTimer <= 0)
            return EnemyStateMachine.EnemyState.Idle;
        else
            return Key;
    } 

    public override void Update() {
        Enemy.AggroTimer -= Time.deltaTime;
        float distanceToAggroTarget = Vector3.Distance(Enemy.transform.position, Enemy.AggroTarget.transform.position);

        if (distanceToAggroTarget < Enemy.AggroRadius) 
            Enemy.AggroTimer = Enemy.AggroTime;

        Enemy.MoveTo(Enemy.AggroTarget.transform.position);
    }
}