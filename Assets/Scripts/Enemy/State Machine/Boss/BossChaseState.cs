using UnityEngine;
using UnityEngine.AI;

public class BossChaseState : BaseState<BossStateMachine.BossState> {
    FidgetSpinner fidgetSpinner;

    bool CanAttack => cooldownTimer >= attackCooldown && fidgetSpinner.DistanceToTarget <= fidgetSpinner.AttackRange;
    
    float attackCooldown = 5f, cooldownTimer = 0f;

    public BossChaseState(BossStateMachine.BossState key, FidgetSpinner fidgetSpinner) : base(key) {
        this.fidgetSpinner = fidgetSpinner;
    }

    public override void Enter() {
        cooldownTimer = 0f;
        fidgetSpinner.SelectTarget();
    }

    public override void Exit() { }

    public override BossStateMachine.BossState GetNextState() => CanAttack ? BossStateMachine.BossState.Attack : Key;

    public override void Update() {
        cooldownTimer += Time.deltaTime;
        fidgetSpinner.MoveTowardsTarget();
    }
}