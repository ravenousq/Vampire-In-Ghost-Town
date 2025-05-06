
public class GarryGroundedState : EnemyState
{
    protected Garry enemy;

    public GarryGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
