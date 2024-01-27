using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyStateMachine))]
public class BasicEnemy : MonoBehaviour {
    [field: SerializeField, Min(0.1f)]
    public float WanderRadius { get; private set; } = 5f;

    [field: SerializeField]
    public Vector3 WanderPivot { get; private set; }

    [field: SerializeField, Min(0.1f)]
    public float Speed { get; private set; } = 1f;

    public bool HasReachedDestination => agent.remainingDistance <= agent.stoppingDistance;

    protected EnemyStateMachine stateMachine;

    protected NavMeshAgent agent;

    protected virtual void Awake() {
        stateMachine = GetComponent<EnemyStateMachine>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;

        WanderPivot = transform.position;
    }

    public void MoveTo(Vector3 destination) => agent.SetDestination(destination);

    public virtual void Attack() {

    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Handles.DrawWireDisc(WanderPivot, Vector3.up, WanderRadius);
    }
}
