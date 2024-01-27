using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy enemy;

    float circleProgress = 0f;

    public EnemyPatrolState(EnemyStateMachine.EnemyState key, BasicEnemy Enemy ) : base(key) {
        this.enemy = Enemy;
    }

    public override void Enter() => Wander();

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() => Key;

    public override void Update() {
        float circleCircumference = 2f * Mathf.PI * enemy.WanderRadius;
        circleProgress += Time.deltaTime * enemy.Speed / circleCircumference;

        if (circleProgress >= 1f)
            circleProgress -= 1f;

        if (enemy.HasReachedDestination)
            Wander();
    }

    void Wander() {
        Vector3? wanderPoint = GetWanderDestination();

        if (wanderPoint.HasValue)
            enemy.MoveTo(wanderPoint.Value);
    }

    Vector3? GetWanderDestination() {
        float sin = Mathf.Sin(circleProgress * Mathf.PI * 2f);
        float cos = Mathf.Cos(circleProgress * Mathf.PI * 2f);

        Vector3 circlePoint = new Vector3(sin, 0f, cos) * enemy.WanderRadius;
        Vector3 destination = enemy.WanderPivot + circlePoint;

        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, enemy.WanderRadius, NavMesh.AllAreas)) {
            return hit.position;
        }
            
        return null;
    }
}