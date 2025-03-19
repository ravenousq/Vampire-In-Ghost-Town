using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idle { get; private set; }
    public PlayerMoveState move { get; private set; }
    public PlayerJumpState jump { get; private set; }
    public PlayerAirborneState airborne { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public  PlayerClimbState climb { get; private set; }
    public PlayerDashState dash { get; private set; }
    #endregion

    [Header("Collision")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [HideInInspector] public bool isBusy;
    [HideInInspector] public bool canWallSlide = true;
    public BoxCollider2D ladderToClimb { get; private set; }

    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float coyoteJumpWindow;
    public float bufferJumpWindow;
    public float wallSlideTime;
    public float climbSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    private float lastDash;

    [Header("Prefabs")]
    [SerializeField] private GameObject afterImage;
    #region Flags
    [HideInInspector] public bool playStartAnim = true;
    [HideInInspector] public bool allowCoyote;
    [HideInInspector] public bool executeBuffer;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool creatingAfterImage;
    #endregion


    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        stateMachine = new PlayerStateMachine();
        idle = new PlayerIdleState(this, stateMachine, "idle");
        move  = new PlayerMoveState(this, stateMachine, "move");    
        jump = new PlayerJumpState(this, stateMachine, "jump");
        airborne = new PlayerAirborneState(this, stateMachine, "jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "jump");
        climb = new PlayerClimbState(this, stateMachine, "climb");
        dash = new PlayerDashState(this, stateMachine, "dash");
    }

    void Start()
    {
        stateMachine.Initialize(idle);
    }

    void Update()
    {
        stateMachine.current.Update();

        if(!playStartAnim)
            Invoke(nameof(ResetMoveStart), .5f);
    }

    private void LateUpdate() 
    {
        CheckForDashInput();
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;

        FlipController(velocity.x);
    }

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new Vector2(x, y);

        FlipController(x);
    }

    public void ResetVelocity() => rb.linearVelocity = Vector2.zero;

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }

    public void FlipController(float x)
    {
        if ((facingRight && x < 0) || (!facingRight && x > 0))
            Flip();
    }

    private void ResetMoveStart() => playStartAnim = true;

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CheckForDashInput()
    {
        if(!canDash || lastDash > Time.time - dashCooldown)
            return;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            lastDash = Time.time;
            stateMachine.ChangeState(dash);
            creatingAfterImage = true;
            InvokeRepeating(nameof(CreateAfterImage), 0, .02f);
        }
    }

    public void PostDash() => StartCoroutine(ZeroGravityFor(.1f));

    private IEnumerator ZeroGravityFor(float seconds)
    {
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(seconds);

        rb.gravityScale = gravity;
    }

    public void TriggerLadder(BoxCollider2D ladder) => ladderToClimb = ladder;

    public void ReenableDash()
    {
        canDash = true;
        lastDash = 0;
    }

    private void CreateAfterImage()
    {
        if(!creatingAfterImage)
        {
            CancelInvoke(nameof(CreateAfterImage));
            return;
        }

        GameObject newAfterImage = Instantiate(afterImage, transform.position, Quaternion.identity);
        newAfterImage.GetComponent<AfterImage>().SetUpSprite(sr.sprite, facingRight);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
