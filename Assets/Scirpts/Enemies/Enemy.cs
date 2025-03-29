using System.Diagnostics;
using Unity.VisualScripting;
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

    [Header("Combat")]
    public float attackDistance;
    public float attackCooldown;
    public float lastTimeAttacked { get; private set; }
    public Transform attackPoint;

    public System.Action onDamaged;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();

        SwitchKnockability();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();
    }

    public override void Damage()
    {
        base.Damage();

        if(onDamaged != null)
            onDamaged();
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

    public virtual void Dodamage() => Physics2D.OverlapCircle(attackPoint.position, attackDistance, whatIsPlayer).gameObject.GetComponent<Player>().Damage();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.black;

        Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x + (aggroRange * facingDir), transform.position.y - .1f));
        Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x - (aggroRange/2 * facingDir), transform.position.y - .1f));

        //Gizmos.color = Color.green;

        //Gizmos.DrawLine(transform.position + new Vector3(0, .1f), new Vector3(transform.position.x + (attackDistance * facingDir), transform.position.y + .1f));

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
