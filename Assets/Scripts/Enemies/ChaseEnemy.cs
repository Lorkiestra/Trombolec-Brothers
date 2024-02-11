using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class ChaseEnemy : BasicEnemy {
    [field: SerializeField, Min(0.1f)]
    public float AggroSpeedMultiplier { get; private set; } = 2f;
    
    [field: SerializeField]
    public float AggroRadius { get; private set; } = 5f;

    [field: SerializeField, Min(0.1f)]
    public float AggroTime { get; private set; } = 5f;

    [field: SerializeField]
    public float AttackRange { get; private set; } = 1f;

    [field: SerializeField]
    public float AttackSpeed { get; private set; } = 1f;

    public bool IsAggroed { get; private set; }
    
    public bool IsInAttackRangeRange => Vector3.Distance(transform.position, AggroTarget.transform.position) <= AttackRange;

    public float AggroTimer { get; set; } = 0f;

    public float AttackTimer { get; set; } = 0f;

    public Brother AggroTarget { get; protected set; }

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

    public void SetAggro() {
        IsAggroed = true;
        AggroTimer = AggroTime;
        agent.speed *= AggroSpeedMultiplier;
        agent.SetDestination(AggroTarget.transform.position);
    }

    public void ClearAggro() {
        IsAggroed = false;
        AggroTimer = 0f;
        agent.speed /= AggroSpeedMultiplier;
        agent.SetDestination(transform.position);
    }

    private void HandleTargetChange(Brother brother) {
        float distanceToNewBrother = Vector3.Distance(transform.position, brother.transform.position);
        float distanceToFocusedBrother = Vector3.Distance(transform.position, AggroTarget.transform.position);

        if (distanceToNewBrother < distanceToFocusedBrother)
            AggroTarget = brother;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Brother brother)) {
            AggroTarget = brother;
            stateMachine.TransitionTo(EnemyStateMachine.EnemyState.Chase);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.TryGetComponent(out Brother brother))
            HandleTargetChange(brother);
    }
}
