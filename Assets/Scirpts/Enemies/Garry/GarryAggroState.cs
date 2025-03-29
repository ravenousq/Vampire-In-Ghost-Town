using UnityEngine;

public class GarryAggroState : GarryGroundedState
{
    private Player player;
    private bool playerGone;
    private float attackTimer;

    public GarryAggroState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;
        
        enemy.anim.speed = 1.5f;

        enemy.onDamaged -= enemy.BecomeAggresive;

        attackTimer = Physics2D.OverlapCircle(enemy.attackPoint.position, enemy.attackDistance/ 2, enemy.whatIsPlayer) ? enemy.attackCooldown : 0;
    }

    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;

        if(enemy.IsPlayerDetected())
        {
            stateTimer = 1f;
            playerGone = false;
        }

        if(!enemy.IsPlayerDetected() && !playerGone)
        {
            playerGone = true;
            stateTimer = enemy.aggroTime;
        }
        
        if(!Physics2D.OverlapCircle(enemy.attackPoint.position, enemy.attackDistance/ 2, enemy.whatIsPlayer))
            enemy.SetVelocity(enemy.movementSpeed * playerOnRight() * 1.5f, rb.linearVelocityY);
        else if(Physics2D.OverlapCircle(enemy.attackPoint.position, enemy.attackDistance/ 2, enemy.whatIsPlayer) && attackTimer > 0)
            enemy.ResetVelocity();
        else
            stateMachine.ChangeState(enemy.attack);

        if(stateTimer < 0 && !enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.idle);
    }

    public override void Exit()
    {
        base.Exit();

        if(enemy.patrolRoute && !enemy.patrolRoute.bounds.Contains(enemy.transform.position))
            enemy.patrolRoute = null;

        enemy.anim.speed = 1f;

        enemy.ResetVelocity();
    } 

    private int playerOnRight() => player.transform.position.x > enemy.transform.position.x ? 1 : -1;

}
