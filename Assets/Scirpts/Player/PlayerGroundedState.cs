using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        player.canWallSlide = true;
        player.skills.dash.SwitchBlockade(false);
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKey(KeyCode.S) && !player.isBusy)
            stateMachine.ChangeState(player.crouch);

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(player.CanShoot())
                stateMachine.ChangeState(player.attack);
            else
                stateMachine.ChangeState(player.reload);
        }
        
        if(Input.GetKeyDown(KeyCode.F) && player.skills.wanted.CanUseSkill())
            stateMachine.ChangeState(player.aimGun);

        if(Input.GetKeyDown(KeyCode.Mouse1))
            stateMachine.ChangeState(player.quickstep);

        if(Input.GetKeyDown(KeyCode.R) && player.CanReload())
            stateMachine.ChangeState(player.reload);

        if((Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) || player.executeBuffer)
            stateMachine.ChangeState(player.jump);

        if(player.facingDir == xInput && player.IsWallDetected())
            xInput = 0;

        if(!player.IsGroundDetected())
        {
            player.allowCoyote = true;
            stateMachine.ChangeState(player.airborne);
        }

        if(yInput == 1 && player.ladderToClimb && !player.isBusy)
            stateMachine.ChangeState(player.climb);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
