using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyStateMachine))]
public class BasicEnemy : MonoBehaviour {
    [field: SerializeField, Min(0.1f)]
    public float WanderRange { get; private set; } = 1f;

    [field: SerializeField, Min(0.1f)]
    public float Speed { get; private set; } = 1f;

    [field: SerializeField]
    public float AggroRadius { get; private set; } = 5f;

    [field: SerializeField, Min(0.1f)]
    public float AggroSpeedMultiplier { get; private set; } = 2f;

    [field: SerializeField, Min(0.1f)]
    public float AggroTime { get; private set; } = 5f;

    [field: SerializeField]
    public float AttackRange { get; private set; } = 1f;

    [field: SerializeField]
    public float AttackSpeed { get; private set; } = 1f;

    public bool IsAggroed { get; private set; }

    public bool HasReachedDestination => agent.remainingDistance <= agent.stoppingDistance;

    public bool IsInAttackRangeRange => Vector3.Distance(transform.position, AggroTarget.transform.position) <= AttackRange;

    public float AggroTimer { get; set; } = 0f;

    public float AttackTimer { get; set; } = 0f;

    public Brothers AggroTarget { get; protected set; }

    protected EnemyStateMachine stateMachine;

    protected NavMeshAgent agent;

    protected virtual void Awake() {
        stateMachine = GetComponent<EnemyStateMachine>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    public void MoveTo(Vector3 destination) => agent.SetDestination(destination);

    public void SetAggro() {
        IsAggroed = true;
        AggroTimer = AggroTime;
        agent.speed *= AggroSpeedMultiplier;
        agent.SetDestination(AggroTarget.transform.position);
    }

    public void ClearAggro() {
        AggroTarget = null;
        IsAggroed = false;
        AggroTimer = 0f;
        agent.speed /= AggroSpeedMultiplier;
        agent.SetDestination(transform.position);
    }

    public virtual void Attack() {

    }
}
