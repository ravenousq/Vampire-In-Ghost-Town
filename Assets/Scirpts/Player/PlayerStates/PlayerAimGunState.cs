using UnityEngine;
public class PlayerAimGunState : PlayerState
{
    public PlayerAimGunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        rb.bodyType = RigidbodyType2D.Kinematic;

        stateTimer = player.skills.wanted.GetMaxDuration();
        player.skills.dash.SwitchBlockade(true);
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if(stateTimer < 0 || !player.crosshair)
        {
            stateMachine.ChangeState(player.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.bodyType = RigidbodyType2D.Dynamic;;

        player.skills.dash.SwitchBlockade(false);
    }
}
