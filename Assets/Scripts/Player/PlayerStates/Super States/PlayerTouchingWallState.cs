using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    private bool isGrounded;
    private bool isTouchingWall;
    protected int yInput;
    protected bool jumpInput;
    protected bool grapInput;
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData,animationName)
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
        player.PlayerDashStateTwo.ResetDashState();
    }


    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        yInput = player.InputHandler.NormInputY;
        grapInput = player.InputHandler.GrapInput;
        jumpInput = player.InputHandler.JumpInput;
        if (!isExitingState)
        {
            if (jumpInput)
            {
                player.PlayerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.PlayerWallJumpState);
            }
            else if (isGrounded && !grapInput)
            {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
            else if (!isTouchingWall || (player.InputHandler.MovementInput.x != player.FacingDirection && !grapInput))
            {
                stateMachine.ChangeState(player.PlayerInAirState);
            }
        }
    }
}
