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

    public bool HasReachedDestination => agent.remainingDistance <= agent.stoppingDistance;

    protected EnemyStateMachine stateMachine;

    protected NavMeshAgent agent;

    protected virtual void Awake() {
        stateMachine = GetComponent<EnemyStateMachine>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    public void MoveTo(Vector3 destination) => agent.SetDestination(destination);

    public virtual void Attack() {

    }
}
