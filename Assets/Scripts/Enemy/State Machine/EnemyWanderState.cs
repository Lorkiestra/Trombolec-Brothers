using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy Enemy;

    LayerMask groundLayer;

    public EnemyWanderState(EnemyStateMachine.EnemyState key, BasicEnemy Enemy, LayerMask groundLayer) : base(key) {
        this.Enemy = Enemy;
        this.groundLayer = groundLayer;
    }

    public override void Enter() => Wander();

    public override void Exit() { }

    public override EnemyStateMachine.EnemyState GetNextState() => Enemy.HasReachedDestination ? EnemyStateMachine.EnemyState.Idle : Key;

    public override void Update() { }

    void Wander() {
        Vector3? wanderPoint = GetWanderDestination();

        if (wanderPoint.HasValue)
            Enemy.MoveTo(wanderPoint.Value);
    }

    Vector3? GetWanderDestination() {
        Vector3 randomDir = Random.insideUnitCircle.normalized;
        Vector3 wanderPoint = Enemy.transform.position + (randomDir * Enemy.WanderRange);

        if (Physics.Raycast(wanderPoint + Vector3.up, -Enemy.transform.up, 10f, groundLayer))
            return wanderPoint;
        else
            return null;
    }
}