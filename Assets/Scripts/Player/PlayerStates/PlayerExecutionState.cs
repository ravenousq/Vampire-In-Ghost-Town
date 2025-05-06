using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerExecutionState : PlayerState
{
    public Enemy target;
    int playerLayer = LayerMask.NameToLayer("Player");
    int enemyLayer = LayerMask.NameToLayer("Enemy");
    Vector2 targetPosition;
    private bool canHeal;
    private int healAmmount;

    public PlayerExecutionState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skills.dash.SwitchBlockade(true);
        player.skills.halo.SwitchBlockade(true);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        player.stats.SwitchInvincibility(false);

        
        target.GetExecuted();
        targetPosition = new Vector2(target.transform.position.x + target.attackDistance * (player.transform.position.x < target.transform.position.x ? -1 : 1), player.transform.position.y);
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if (Vector2.Distance(player.transform.position, targetPosition) > 0.1f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, targetPosition, 15 * Time.deltaTime);
            return;
        }

        if(player.attackTrigger)
        {
            player.attackTrigger = false;
            player.InstantiateFX(player.shootFX, player.wallCheck, new Vector3(2 * player.facingDir, .6f), new Vector3(0, player.facingDir == -1 ? 0 : 180, -270));
            target.stats.Invoke(nameof(target.stats.Recover), .5f);
            target.stats.TakeDamage(player.executionDamage);

            if(canHeal)
                player.stats.Heal(healAmmount);
        }

        if(trigger)
        {
            player.enemyToExecute.AllowExecution(false);
            player.enemyToExecute = null;
            stateMachine.ChangeState(player.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.SwitchInvincibility(true);
        target = null;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        player.skills.dash.SwitchBlockade(false);
        player.skills.halo.SwitchBlockade(false);
    }

    public void ModifyHealing(bool canHeal, int healAmmount)
    {
        this.canHeal = canHeal;
        this.healAmmount = healAmmount;
    }
}
