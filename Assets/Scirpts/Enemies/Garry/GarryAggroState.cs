using UnityEngine;

public class GarryAggroState : GarryGroundedState
{
    private Player player;
    private bool playerGone;
    private float attackTimer;
    private float aggroMultiplayer = 2;
    private float randomizedDistance;

    public GarryAggroState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.stats.OnDamaged -= enemy.BecomeAggresive;

        player = PlayerManager.instance.player;
        randomizedDistance = Random.Range(enemy.attackDistance /1.5f, enemy.attackDistance /3);

        enemy.anim.speed = aggroMultiplayer;
        playerGone = false;

        attackTimer = Physics2D.OverlapCircle(enemy.attackPoint.position, enemy.attackDistance/ 2, enemy.whatIsPlayer) ? enemy.attackCooldown : 1;
    }

    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;

        if(enemy.IsPlayerDetected())
        {
            stateTimer = 10f;
            playerGone = false;
        }

        if(!enemy.IsPlayerDetected() && !playerGone)
        {
            playerGone = true;
            stateTimer = enemy.aggroTime;
        }

        if(!Physics2D.OverlapCircle(enemy.attackPoint.position, randomizedDistance, enemy.whatIsPlayer))
            enemy.SetVelocity(enemy.movementSpeed * playerOnRight() * aggroMultiplayer, rb.linearVelocityY);
        else
        {
            if(attackTimer > 0)
                enemy.ResetVelocity();
            else
                stateMachine.ChangeState(enemy.attack);
        }

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
