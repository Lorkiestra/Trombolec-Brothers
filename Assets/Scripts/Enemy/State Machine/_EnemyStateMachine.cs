using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicEnemy))]
public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState> {
    public enum EnemyState {
        Idle,
        ResetPatrol,
        Patrol,
        Chase,
        Attack,
        Die
    }

    void Awake() {
        BasicEnemy enemy = GetComponent<BasicEnemy>();

        States = new Dictionary<EnemyState, BaseState<EnemyState>> {
            { EnemyState.ResetPatrol, new EnemyResetPatrolState(EnemyState.ResetPatrol, enemy)},
            { EnemyState.Patrol, new EnemyPatrolState(EnemyState.Patrol, enemy)},
            { EnemyState.Die, new EnemyDieState(EnemyState.Die) },
        };

        if (enemy is ChaseEnemy) {
            States.Add(EnemyState.Chase, new EnemyChaseState(EnemyState.Chase, enemy));
            States.Add(EnemyState.Attack, new EnemyAttackState(EnemyState.Attack, enemy));
        }

        CurrentState = States[EnemyState.ResetPatrol];
    }
}