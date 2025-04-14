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

        skills.dash.SwitchBlockade(true);
        skills.halo.SwitchBlockade(true);
        skills.parry.SwitchBlockade(true);

        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.Heal(skills.concoction.GetHeal());
        
        skills.dash.SwitchBlockade(false);
        skills.halo.SwitchBlockade(false);
        skills.parry.SwitchBlockade(false);
    }
}
