using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
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
    public PlayerPrimaryAttackState attack { get; private set; }
    public PlayerReloadState reload { get; private set; }
    public PlayerQuickstepState quickstep { get; private set; }
    public PlayerCrouchState crouch { get; private set; }
    public PlayerDiveState dive{ get; private set; }
    #endregion


    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float coyoteJumpWindow;
    public float bufferJumpWindow;
    public float wallSlideTime;
    public float wallSlideSpeed;
    public float climbSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    private float lastDash;
    public float quickstepSpeed;
    public float diveSpeed;

    [Header("Combat")]
    public int maxAmmo;
    public  int currentAmmo;
    public float reloadMovementSpeed;
    public float attackWindow;
    public int[] attackMovement;

    [Header("Abilities")]
    public bool voculFenMah;


    [Header("Prefabs")]
    [SerializeField] private GameObject afterImage;

    #region Flags
    [HideInInspector] public bool playStartAnim = true;
    [HideInInspector] public bool allowCoyote;
    [HideInInspector] public bool executeBuffer;
    [HideInInspector] public bool canWallSlide = true;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool creatingAfterImage;
    [HideInInspector] public bool attackTrigger;
    [HideInInspector] public bool floorParry;
    private bool thirdAttack;
    #endregion

    public BoxCollider2D ladderToClimb { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idle = new PlayerIdleState(this, stateMachine, "idle");
        move  = new PlayerMoveState(this, stateMachine, "move");    
        jump = new PlayerJumpState(this, stateMachine, "jump");
        airborne = new PlayerAirborneState(this, stateMachine, "jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "jump");
        climb = new PlayerClimbState(this, stateMachine, "climb");
        dash = new PlayerDashState(this, stateMachine, "dash");
        attack = new PlayerPrimaryAttackState(this, stateMachine, "attack");
        reload = new PlayerReloadState(this, stateMachine, "placeholder");
        quickstep = new PlayerQuickstepState(this, stateMachine, "placeholder");
        crouch = new PlayerCrouchState(this, stateMachine, "placeholder");
        dive = new PlayerDiveState(this, stateMachine, "placeholder");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idle);

        Reload();
    }

    protected override void Update()
    {

        stateMachine.current.Update();

        if(!playStartAnim)
            Invoke(nameof(ResetMoveStart), .5f);
    }

    private void LateUpdate() 
    {
        CheckForDashInput();
    }

    private void ResetMoveStart() => playStartAnim = true;

    public void TriggerLadder(BoxCollider2D ladder) => ladderToClimb = ladder;

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

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public bool CanReload()
    {
        if(currentAmmo < maxAmmo)
            return true;
        
        return false;
    }

    public void ModifyBullets(int bullets)
    {
        currentAmmo += bullets;

        if(currentAmmo < 0)
            currentAmmo = 0;
        else if(currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
    }

    public void ThirdAttack() => StartCoroutine(ThirdAttackRoutine());

    private IEnumerator ThirdAttackRoutine()
    {
        thirdAttack = true;

        yield return new WaitForSeconds(attackWindow);

        thirdAttack = false;
    }

    public bool CanShoot()
    {
        if(currentAmmo > 0 && !thirdAttack)
            return true;
        else if(currentAmmo > 0 && thirdAttack)
            return true;

        return false;
    }

    public bool CloseToEdge()
    {
        Vector2 edgeCheck = groundCheck.transform.position;
        int edgeToCheck = rb.linearVelocityX > 0 ? 1 : -1;

        edgeCheck += new Vector2(cd.size.x / 2 * edgeToCheck, 0);

        if(Physics2D.OverlapCircle(edgeCheck, .3f, whatIsGround))
            return false;

        return true;
    }
}
