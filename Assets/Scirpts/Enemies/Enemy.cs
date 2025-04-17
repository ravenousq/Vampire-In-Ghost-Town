using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    #region Components
    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyStats stats { get; private set; }
    public GameObject mark;
    #endregion

    [Header("Movement")]
    public float movementSpeed;
    public float idleTime;
    public float aggroTime;

    [Header("Detection")]
    [SerializeField] protected float aggroRange;
    public LayerMask whatIsPlayer;
    public float executionRange;

    [Header("Combat")]
    public int poiseDamage;
    public float attackDistance;
    public float attackCooldown;
    public Vector2 attackKnockback;
    public Transform attackPoint;
    public bool canBeStunned { get; private set; }
    public bool canBeExecuted { get; private set; }

    [Header("Drop")]
    [SerializeField] private ItemObject itemPrefab;
    [SerializeField] private ItemData drop;
    [SerializeField] private Transform itemDropPosition;

    Player player;

    public System.Action onDamaged;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;

        stats = GetComponent<EnemyStats>();
        SwitchKnockability();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();

        if(canBeExecuted)
            ControlExecution();
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
                Debug.Log("Perfect Dash");

                player.SlowDownTime();

                Destroy(hit.gameObject);

                int currentBullets = player.skills.shoot.currentAmmo;

                int bulletsToRefill = Mathf.RoundToInt((12 - currentBullets)/ 2);
                player.skills.shoot.ModifyBullets(bulletsToRefill);
            }
        }
    }

    public override void Die()
    {
        base.Die();

        if(drop)
            Instantiate(itemPrefab, itemDropPosition.position, Quaternion.identity).SetUpItem(drop);

        Destroy(gameObject);
    }

    public override void Flip()
    {
        StartCoroutine(StopMovingFor(.6f));

        base.Flip();
    }

    public override void Stun()
    {
        base.Stun();

        cd.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Static;

        AllowExecution(true);
    }

    public virtual void Recover() 
    {
        cd.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    protected IEnumerator StopMovingFor(float seconds)
    {
        canMove = false;

        yield return new WaitForSeconds(seconds);

        canMove = true;
    }

    #region Parry
    public void OpenParryWindow() => canBeStunned = true;

    public virtual void Parried() => CloseParryWindow();

    public void CloseParryWindow() => canBeStunned = false;
    #endregion

    #region Execution
    public virtual void GetExecuted()
    {
        canBeExecuted = false;
        FlipController(PlayerManager.instance.player.transform.position.x - transform.position.x);
    }

    public virtual void AllowExecution(bool allow) 
    {
        canBeExecuted = allow;
        if(player.enemyToExecute == this)
            player.AssignExecutionTarget(null);
    }

    protected virtual void ControlExecution()
    {
        if(Physics2D.OverlapCircle(transform.position, executionRange, whatIsPlayer) && player.stateMachine.current == player.dash && SkillManager.instance.isSkillUnlocked("Dance Macabre"))
        {
            player.enemyToExecute = null;
            player.NoCollisionsFor(.5f);
            canBeExecuted = false;

            if(stats.HP <= stats.health.GetValue() * .3f)
            {
                stats.Die();
                return;
            }

            stats.TakeDamage(player.executionDamage);
            stats.Invoke(nameof(Recover), .5f);
        }
        else
            player.AssignExecutionTarget(this);
    }

    #endregion

    #region Detection 
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

    public virtual void BecomeAggresive()
    {
        if(IsAlreadyAggresive() || canBeExecuted)
            return;
    }

    public virtual bool  IsAlreadyAggresive()
    {
        return false;
    }

    #endregion

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
