using UnityEngine;

public class PlayerDialogueState : PlayerState
{
    public PlayerDialogueState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skills.ChangeLockOnAllSkills(true);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        skills.ChangeLockOnAllSkills(false);
    }

    public void EndDialogue() => stateMachine.ChangeState(player.idle);
}
