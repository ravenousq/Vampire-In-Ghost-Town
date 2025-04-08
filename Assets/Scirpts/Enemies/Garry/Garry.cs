using UnityEditor;
using UnityEngine;

[SelectionBase]
public class Garry : Enemy
{
    #region States
    public GarryIdleState idle { get; private set; }
    public GarryMoveState move { get; private set; }
    public GarryAggroState aggro { get; private set; }
    public GarryAttackState attack { get; private set; }
    public GarryStunnedState stun { get; private set; }
    #endregion

    [Header("Patrol")]
    public EdgeCollider2D patrolRoute = null;
    private EdgeCollider2D possibleRoute;
    [SerializeField] private LayerMask whatIsRoute;

    protected override void Awake()
    {
        base.Awake();

        idle = new GarryIdleState(this, stateMachine, "idle", this);
        move = new GarryMoveState(this, stateMachine, "move", this);
        aggro = new GarryAggroState(this, stateMachine, "move", this);
        attack = new GarryAttackState(this, stateMachine, "attack", this);
        stun = new GarryStunnedState(this, stateMachine, "stun", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log(stateMachine.current.animBoolName);

        stateMachine.current.Update();

        AssignPatrolPath();
    }

    private void AssignPatrolPath()
    {
        if (possibleRoute && Vector2.Distance(transform.position, possibleRoute.gameObject.transform.position) < 2f)
        {
            patrolRoute = possibleRoute;
            possibleRoute = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(((1<<other.gameObject.layer) & whatIsRoute) != 0)
            possibleRoute = other.GetComponent<EdgeCollider2D>();    

        if(((1<<other.gameObject.layer) & whatIsPlayer) != 0) 
            BecomeAggresive();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(((1<<other.gameObject.layer) & whatIsPlayer) != 0) 
            BecomeAggresive();
    }

    public override void BecomeAggresive()
    {
        if(isAlreadyAggresive() || stateMachine.current == stun)
            return;

        stateMachine.ChangeState(aggro);
    }

    public override bool isAlreadyAggresive()
    {
        if(stateMachine.current == aggro || stateMachine.current == attack)
            return true;

        return false;
    }

    public override void Recover()
    {
        base.Recover();

        if(stateMachine.current == stun)
            stateMachine.ChangeState(idle);
    }

    public override void Stun()
    {
        base.Stun();

        stateMachine.ChangeState(stun);
    }

    public override void GetExecuted()
    {
        base.GetExecuted();

        stateMachine.ChangeState(stun);
    }

    public override void Parried()
    {
        base.Parried();

        if(!stats.isStunned)
            stateMachine.ChangeState(idle);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(!EditorApplication.isPlaying)
            return;

        if(stateMachine.current == stun)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, executionRange);
        }

        if(stateMachine.current == attack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
        }

        if(stateMachine.current == idle || stateMachine.current == move)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x + (aggroRange * facingDir), transform.position.y - .1f));
            Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x - (aggroRange/2 * facingDir), transform.position.y - .1f));
        }
    }
}
