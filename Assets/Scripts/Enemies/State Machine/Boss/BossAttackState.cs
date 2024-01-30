using UnityEngine;
using UnityEngine.AI;

public class BossAttackState : BaseState<BossStateMachine.BossState> {
    private FidgetSpinner fidgetSpinner;

    private float targetRotationSpeedMultiplier = 10f, speedMultiplier = 4f, windupTime = 3f, windupTimer = 0f;

    private bool isAttacking = false;

    public BossAttackState(BossStateMachine.BossState key, FidgetSpinner fidgetSpinner) : base(key) {
        this.fidgetSpinner = fidgetSpinner;
    }

    public override void Enter() {
        windupTimer = 0f;
        isAttacking = true;

        fidgetSpinner.TargetRotationSpeed *= targetRotationSpeedMultiplier;
        fidgetSpinner.Speed *= speedMultiplier;

        fidgetSpinner.LockInChargeTarget();
    }

    public override void Exit() {
        isAttacking = false;
        fidgetSpinner.TargetRotationSpeed /= targetRotationSpeedMultiplier;
        fidgetSpinner.Speed /= speedMultiplier;
    }

    public override BossStateMachine.BossState GetNextState() => isAttacking ? Key : BossStateMachine.BossState.Chase;

    public override void Update() {
        windupTimer += Time.deltaTime;

        if (windupTimer < windupTime) return;

        if (fidgetSpinner.DistanceToChargeTarget < 1f)
            isAttacking = false;

        fidgetSpinner.ChargeAtTarget();
    }
}