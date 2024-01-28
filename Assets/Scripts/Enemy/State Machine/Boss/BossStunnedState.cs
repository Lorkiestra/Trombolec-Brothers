using UnityEngine;
using UnityEngine.AI;

public class BossStunnedState : BaseState<BossStateMachine.BossState> {
    private FidgetSpinner fidgetSpinner;

    private float loweredTargetHoverHeight = 1f, oldTargetHoverHeight, rotationSpeedMultiplier = .25f, stunTime = 7f, stunTimer = 0f;

    public BossStunnedState(BossStateMachine.BossState key, FidgetSpinner fidgetSpinner) : base(key) {
        this.fidgetSpinner = fidgetSpinner;
    }

    public override void Enter() {
        stunTimer = 0f;
        oldTargetHoverHeight = fidgetSpinner.TargetHoverHeight;
        fidgetSpinner.TargetHoverHeight = loweredTargetHoverHeight;
        fidgetSpinner.TargetRotationSpeed *= rotationSpeedMultiplier;
    } 

    public override void Exit() {
        fidgetSpinner.TargetHoverHeight = oldTargetHoverHeight;
        fidgetSpinner.TargetRotationSpeed /= rotationSpeedMultiplier;
    }

    public override BossStateMachine.BossState GetNextState() => stunTimer >= stunTime ? BossStateMachine.BossState.Chase : Key;

    public override void Update() {
        stunTimer += Time.deltaTime;
        fidgetSpinner.AdjustHeight();
    }
}