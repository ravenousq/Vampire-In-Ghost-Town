
public class PlayerImpactState : PlayerState
{
    public PlayerImpactState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skills.ChangeLockOnAllSkills(true);
        player.stats.SwitchInvincibility(true);
    }

    public override void Update()
    {
        base.Update();

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        skills.ChangeLockOnAllSkills(false);
        player.stats.SwitchInvincibility(false);
    }
}
