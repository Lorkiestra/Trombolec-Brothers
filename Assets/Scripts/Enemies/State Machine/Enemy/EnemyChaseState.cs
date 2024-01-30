using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : BaseState<EnemyStateMachine.EnemyState> {
    ChaseEnemy enemy;

    public EnemyChaseState(EnemyStateMachine.EnemyState key, BasicEnemy enemy) : base(key) {
        this.enemy = (ChaseEnemy)enemy;
    }

    public override void Enter() => enemy.SetAggro();

    public override void Exit() => enemy.ClearAggro();

    public override EnemyStateMachine.EnemyState GetNextState() {
        if (enemy.IsInAttackRangeRange) 
            return EnemyStateMachine.EnemyState.Attack;
        else if (enemy.AggroTimer <= 0)
            return EnemyStateMachine.EnemyState.ResetPatrol;
        else
            return Key;
    } 

    public override void Update() {
        enemy.AggroTimer -= Time.deltaTime;
        float distanceToAggroTarget = Vector3.Distance(enemy.transform.position, enemy.AggroTarget.transform.position);

        if (distanceToAggroTarget < enemy.AggroRadius) 
            enemy.AggroTimer = enemy.AggroTime;

        enemy.MoveTo(enemy.AggroTarget.transform.position);
    }
}