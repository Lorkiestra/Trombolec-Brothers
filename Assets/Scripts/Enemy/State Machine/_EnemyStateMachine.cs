using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicEnemy))]
public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState> {
    public enum EnemyState {
        Idle,
        Wander,
        Chase,
        Attack,
        Die
    }

    [SerializeField]
    private LayerMask groundLayer;

    void Awake() {
        BasicEnemy enemy = GetComponent<BasicEnemy>();

        States = new Dictionary<EnemyState, BaseState<EnemyState>> {
            { EnemyState.Idle, new EnemyIdleState(EnemyState.Idle)},
            { EnemyState.Wander, new EnemyWanderState(EnemyState.Wander, enemy, groundLayer)},
            { EnemyState.Chase, new EnemyChaseState(EnemyState.Chase, enemy)},
            { EnemyState.Attack, new EnemyAttackState(EnemyState.Attack, enemy)},
            { EnemyState.Die, new EnemyDieState(EnemyState.Die) },
        };

        CurrentState = States[EnemyState.Idle];
    }
}