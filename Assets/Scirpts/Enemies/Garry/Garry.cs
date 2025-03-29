using UnityEngine;

[SelectionBase]
public class Garry : Enemy
{
    #region States
    public GarryIdleState idle { get; private set; }
    public GarryMoveState move { get; private set; }
    public GarryAggroState aggro { get; private set; }
    public GarryAttackState attack { get; private set; }
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
        attack = new GarryAttackState(this, stateMachine, "idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();

        if(possibleRoute && Vector2.Distance(transform.position, possibleRoute.gameObject.transform.position) < 2f)
        {
            patrolRoute = possibleRoute;
            possibleRoute = null;
        }
    }

    public void BecomeAggresive() => stateMachine.ChangeState(aggro);

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
}
