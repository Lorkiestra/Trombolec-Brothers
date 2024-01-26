using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class ChaseEnemy : BasicEnemy {
    private SphereCollider aggroTrigger;

    protected override void Awake() {
        base.Awake();
        aggroTrigger = GetComponent<SphereCollider>();
        aggroTrigger.isTrigger = true;
    }

    private void OnValidate() {
        if (aggroTrigger == null)
            aggroTrigger = GetComponent<SphereCollider>();
        
        aggroTrigger.radius = AggroRadius;
    }

    private void OnTriggerEnter(Collider other) {
        if (AggroTarget == null && other.TryGetComponent(out Brothers brothers)) {
            AggroTarget = brothers;
            stateMachine.TransitionTo(EnemyStateMachine.EnemyState.Chase);
        }
    }
}
