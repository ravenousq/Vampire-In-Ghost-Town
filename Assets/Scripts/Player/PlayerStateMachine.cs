

public class PlayerStateMachine
{
    public PlayerState current { get; private set; }

    public void Initialize(PlayerState startState)
    {
        current = startState;
        current.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        current.Exit();
        current = newState;
        current.Enter();
    }
}
