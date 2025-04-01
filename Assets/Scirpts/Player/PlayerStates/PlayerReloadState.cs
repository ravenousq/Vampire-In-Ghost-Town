using UnityEngine;

public class PlayerReloadState : PlayerState
{
    public PlayerReloadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.anim.SetInteger("facingDir", player.facingDir);
        player.reloadTorso.SetActive(true);
        player.stats.OnDamaged += BlinkWhileReloading;

        player.skills.dash.SwitchBlockade(true);
        player.skills.halo.SwitchBlockade(true);
    }

    public override void Update()
    {
        base.Update();

        player.anim.SetInteger("facingDir", player.facingDir);

        if(!player.isKnocked)
            rb.linearVelocity = new Vector2(xInput * player.reloadMovementSpeed, 0);

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetInteger("facingDir", player.facingDir);

        player.Reload();
        player.stats.OnDamaged -= BlinkWhileReloading;
        player.reloadTorso.SetActive(false);

        player.skills.dash.SwitchBlockade(false);
        player.skills.halo.SwitchBlockade(false);
    }

    private void BlinkWhileReloading() => player.reloadTorso.GetComponent<FX>().IFramesFlashing(player.stats.iFrames);
    
}
