
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState CurrentState { get; private set; }

    public void Initalize(PlayerState startingState) {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
