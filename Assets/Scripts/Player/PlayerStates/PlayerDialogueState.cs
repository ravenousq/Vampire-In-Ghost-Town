using System;
using UnityEngine;

public class PlayerDialogueState : PlayerState
{
    private bool moving = true;
    public PlayerDialogueState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skills.ChangeLockOnAllSkills(true);
        moving = true;
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if(!moving && Mathf.Abs(player.transform.position.x - player.dialoguePoint.position.x) > .5f)
            return;

        moving = Mathf.Abs(player.transform.position.x - player.dialoguePoint.position.x) > .5f;
        
        if(moving)
        {
            player.anim.SetBool("idle", false);
            player.anim.SetBool("move", true);
            player.transform.position = new Vector3(Mathf.MoveTowards(player.transform.position.x, player.dialoguePoint.position.x, player.movementSpeed * .7f * Time.unscaledDeltaTime), player.transform.position.y); 
            
            if(player.facingDir * (player.dialoguePoint.position.x - player.transform.position.x) < 0)
                player.Flip();
        }
        else
        {
            player.anim.SetBool("move", false);
            player.anim.SetBool("idle", true);

            if(player.facingDir != player.dialogueFacingDir)
                player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        skills.ChangeLockOnAllSkills(false);
    }
}
