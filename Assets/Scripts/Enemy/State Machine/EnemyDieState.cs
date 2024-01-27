public class EnemyDieState : BaseState<EnemyStateMachine.EnemyState> {
    BasicEnemy enemy;

    public EnemyDieState(EnemyStateMachine.EnemyState key, BasicEnemy enemy) : base(key) {
        this.enemy = enemy;
    }

    public override void Enter() { }

    public override void Exit() => enemy.Die();

    public override EnemyStateMachine.EnemyState GetNextState() => Key;

    public override void Update() { }
}