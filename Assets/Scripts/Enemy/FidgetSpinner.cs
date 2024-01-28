using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossStateMachine))]
public class FidgetSpinner : MonoBehaviour {
    [SerializeField]
    private Transform model;

    [field: SerializeField]
    public float Speed { get; set; } = 5f;

    [field: SerializeField]
    public float AttackRange { get; private set; } = 10f;

    [field: SerializeField]
    public float TargetHoverHeight { get; set; } = 4f;

    [field: SerializeField]
    public float TargetRotationSpeed { get; set; } = 60f;

    [field: SerializeField]
    public float StunTime { get; private set; } = 7f;

    [SerializeField]
    List<FidgetSpinnerWeakPoint> weakPoints;

    public Brothers TargetBrother { get; private set; }

    public float DistanceToTarget => Vector3.Distance(transform.position, TargetBrother.transform.position);

    public float DistanceToChargeTarget => Vector3.Distance(transform.position, chargeTargetPoint);

    public bool IsDead => weakPoints.All(weakPoint => weakPoint.isDestroyed);

    private float rotationSpeed;

    private Vector3 chargeTargetPoint;

    BossStateMachine stateMachine;

    private void Awake() {
        stateMachine = GetComponent<BossStateMachine>();
        rotationSpeed = TargetRotationSpeed;
    }

    private void Update() {
        model.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        rotationSpeed = Mathf.Lerp(rotationSpeed, TargetRotationSpeed, Time.deltaTime);
    }

    public void SelectTarget() {
        TargetBrother = Random.Range(0, 2) == 0 ? PukaszTrombolec.Instance : LawelTrombolec.Instance;
    }

    public void AdjustHeight() {
        Vector3 target = transform.position;
        target.y = TargetHoverHeight;

        Vector3 direction = (target - transform.position).normalized;
        Vector3 position = Speed * Time.deltaTime * direction;

        transform.position += position;
    }

    public void MoveTowardsTarget() {
        Vector3 target = TargetBrother.transform.position;
        target.y = TargetHoverHeight;

        Vector3 direction = (target - transform.position).normalized;
        Vector3 position = Speed * Time.deltaTime * direction;

        transform.position += position;
    }

    public void LockInChargeTarget() => chargeTargetPoint = TargetBrother.transform.position;

    public void ChargeAtTarget() {
        Vector3 direction = (chargeTargetPoint - transform.position).normalized;
        Vector3 position = Speed * Time.deltaTime * direction;

        transform.position += position;
    }

    public void Stun() {
        stateMachine.TransitionTo(BossStateMachine.BossState.Stunned);
    }
}
