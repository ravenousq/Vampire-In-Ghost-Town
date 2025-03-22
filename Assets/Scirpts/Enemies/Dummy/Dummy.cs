using UnityEngine;

public class Dummy : Enemy
{
    #region States
    public DummyIdleState idle { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idle = new DummyIdleState(this, stateMachine, "idle", this);
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();
    }
}
