using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine { get; private set; }

    [SerializeField] public GameObject mark;

    [Header("Movement")]
    public float movementSpeed;
    public float idleTime;
    public float aggroTime;

    [Header("Detection")]
    [SerializeField] protected float aggroRange;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public float executionRange;

    [Header("Combat")]
    public int poiseDamage;
    public float attackDistance;
    public float attackCooldown;
    public Vector2 attackKnockback;
    public Transform attackPoint;
    public EnemyStats stats { get; private set; }
    public bool canBeStunned { get; private set; }
    public bool canBeExecuted;

    public System.Action onDamaged;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();

        stats = GetComponent<EnemyStats>();
        SwitchKnockability();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();

        if(canBeExecuted)
        {
            Player player = PlayerManager.instance.player;

            if(Physics2D.OverlapCircle(transform.position, executionRange, whatIsPlayer) && player.stateMachine.current == player.dash && SkillManager.instance.isSkillUnlocked("Dance Macabre"))
            {
                Invoke(nameof(stats.Recover), .5f);
                player.enemyToExecute = null;
                player.NoCollisionsFor(.5f);
                canBeExecuted = false;

                if(stats.HP <= stats.maxHP.GetValue() * .3f)
                {
                    stats.Die();
                    return;
                }
                stats.TakeDamage(player.executionDamage);
            }
            else
                PlayerManager.instance.player.SetExecutionTarget(this);
        }
    }

    public virtual void Parried()
    {
        CloseParryWindow();
    }

    public virtual bool IsPlayerDetected()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * facingDir, aggroRange);
        RaycastHit2D[] hitsBack = Physics2D.RaycastAll(transform.position, Vector2.left * facingDir, aggroRange/2);

        for(int i = 0; i < hits.Length; i++)
        {
            if(((1<<hits[i].collider.gameObject.layer) & whatIsGround) != 0)
                return false;

            if(((1<<hits[i].collider.gameObject.layer) & whatIsPlayer) != 0)
                return true;
        }

        for(int i = 0; i < hitsBack.Length; i++)
        {
            if(((1<<hitsBack[i].collider.gameObject.layer) & whatIsGround) != 0)
                return false;

            if(((1<<hitsBack[i].collider.gameObject.layer) & whatIsPlayer) != 0)
                return true;
        }

        return false;
    } 

    public virtual void DoDamage()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance);

        foreach(var hit in targets)
        {
            if(hit.GetComponent<Player>())
            {
                PlayerStats playerTarget = hit.GetComponent<PlayerStats>();
                stats.DoDamage(playerTarget, attackKnockback, .5f, poiseDamage);
            }

            if(hit.GetComponent<PerfectDashChecker>())
            {
                Destroy(hit.gameObject);

                int currentBullets = PlayerManager.instance.player.currentAmmo;

                int bulletsToRefill = Mathf.RoundToInt((12 - currentBullets)/ 2);
                PlayerManager.instance.player.ModifyBullets(bulletsToRefill);
            }
        }
    }

    public virtual void BecomeAggresive()
    {
        if(isAlreadyAggresive() || canBeExecuted)
            return;
    }

    public virtual bool  isAlreadyAggresive()
    {
        return false;
    }

    public void OpenParryWindow() => canBeStunned = true;

    public void CloseParryWindow() => canBeStunned = false;

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }

    public override void Flip()
    {
        StartCoroutine(StopMovingFor(.6f));

        base.Flip();
    }

    protected virtual void AllowExecution() => canBeExecuted = true;

    public override void Stun()
    {
        base.Stun();

        cd.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Static;

        AllowExecution();
    }

    public virtual void Recover() 
    {
        cd.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public virtual void GetExecuted()
    {
        canBeExecuted = false;
        FlipController(PlayerManager.instance.player.transform.position.x - transform.position.x);
    }

    protected IEnumerator StopMovingFor(float seconds)
    {
        canMove = false;

        yield return new WaitForSeconds(seconds);

        canMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x + (aggroRange * facingDir), transform.position.y - .1f));
        //Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x - (aggroRange/2 * facingDir), transform.position.y - .1f));

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
