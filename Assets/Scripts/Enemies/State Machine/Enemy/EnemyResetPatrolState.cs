using UnityEngine;
using UnityEngine.AI;

public class EnemyResetPatrolState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy enemy;

    public EnemyResetPatrolState(EnemyStateMachine.EnemyState key, BasicEnemy enemy) : base(key) {
        this.enemy = enemy;
    }

    public override void Enter() => GoToPatrolStartPoint();

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() => enemy.HasReachedDestination ? EnemyStateMachine.EnemyState.Patrol : Key;
    
    public override void Update() { }

    void GoToPatrolStartPoint() {
        Vector3 circlePoint = new Vector3(0f, 0f, 1f) * enemy.WanderRadius;
        Vector3 destination = enemy.WanderPivot + circlePoint;

        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, enemy.WanderRadius, NavMesh.AllAreas))
            enemy.MoveTo(hit.position);
        else
            Debug.LogError("Could not find a valid patrol start point", enemy);
    }
}