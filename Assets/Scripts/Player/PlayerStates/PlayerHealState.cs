using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealState : PlayerState
{
    public PlayerHealState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.anim.SetInteger("facingDir", player.facingDir);

        skills.dash.SwitchBlockade(true);
        skills.halo.SwitchBlockade(true);
        skills.parry.SwitchBlockade(true);
        
        player.healTorso.SetActive(true);
        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        player.anim.SetInteger("facingDir", player.facingDir);

        if(!player.isKnocked)
            rb.linearVelocity = new Vector2(xInput * skills.shoot.reloadMovementSpeed, 0);

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetInteger("facingDir", player.facingDir);
        player.healTorso.GetComponent<FX>().ResetSprite();
        player.healTorso.SetActive(false);
        player.stats.Heal(skills.concoction.GetHeal());
        
        skills.dash.SwitchBlockade(false);
        skills.halo.SwitchBlockade(false);
        skills.parry.SwitchBlockade(false);
    }
}
