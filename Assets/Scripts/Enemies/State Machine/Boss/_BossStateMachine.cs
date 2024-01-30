using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FidgetSpinner))]
public class BossStateMachine : StateManager<BossStateMachine.BossState> {
    public enum BossState {
        Chase,
        Attack,
        Stunned
    }

    void Awake() {
        FidgetSpinner fidgetSpinner = GetComponent<FidgetSpinner>();

        States = new Dictionary<BossState, BaseState<BossState>> {
            { BossState.Chase, new BossChaseState(BossState.Chase, fidgetSpinner) },
            { BossState.Attack, new BossAttackState(BossState.Attack, fidgetSpinner) },
            { BossState.Stunned, new BossStunnedState(BossState.Stunned, fidgetSpinner) }
        };

        CurrentState = States[BossState.Chase];
    }
}