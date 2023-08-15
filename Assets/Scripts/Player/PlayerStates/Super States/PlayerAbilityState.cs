
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool AbilityDone;

    private bool isGrounded;
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData,animationName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        AbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        if (AbilityDone)
        {
            // the and condition is when the jump is happen the player is still on ground
            if (isGrounded && player.CurrentVelocity.y < Mathf.Epsilon)
            {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.PlayerInAirState);
            }
        }
        base.LogicalUpdate();
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
