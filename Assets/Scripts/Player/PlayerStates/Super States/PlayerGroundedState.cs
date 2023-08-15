
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 input;
    private bool JumpInput;
    private bool grapInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool dashInput;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = player.CheckIfTouchingWall();
        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        player.PlayerJumpState.ResetAmountOfJumps();
        //player.PlayerDashState.ResetDashState();
        player.PlayerDashStateTwo.ResetDashState();
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        input = player.InputHandler.MovementInput;
        JumpInput = player.InputHandler.JumpInput;
        grapInput = player.InputHandler.GrapInput;
        dashInput = player.InputHandler.DashInput;
        if (JumpInput && player.PlayerJumpState.CheckIfCanJump())
        {
            stateMachine.ChangeState(player.PlayerJumpState);
        }
        else if (!isGrounded)
        {
            player.PlayerInAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.PlayerInAirState);
        }
        else if (isTouchingWall && grapInput)
        {
            stateMachine.ChangeState(player.PlayerWallGrabState);
        }
        /*else if (dashInput && player.PlayerDashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.PlayerDashState);
        }*/
        else if (dashInput && player.PlayerDashStateTwo.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.PlayerDashStateTwo);
        }

    }
}
