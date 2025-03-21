using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.FlipController(attackDir);

        if(lastTimeAttacked < Time.time - player.attackWindow || comboCounter > 2)
            comboCounter = 0;
        
        rb.linearVelocityX = player.attackMovement[comboCounter] * attackDir;
        stateTimer = .1f;

        player.anim.SetInteger("combo", comboCounter);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            player.ResetVelocity();

        if(!player.IsGroundDetected())
            rb.linearVelocityX *= -1;

        if(trigger)
        {
            stateMachine.ChangeState(player.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.15f);
        lastTimeAttacked = Time.time;
        comboCounter++;
    }
}
